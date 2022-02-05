using System.Text;
using System.Text.Json;
using ClosedXML.Excel;

namespace API.Helpers
{
    public class ExcelHelper
    {
        private static readonly byte[] s_nameUtf8 = Encoding.UTF8.GetBytes("name");
        private static readonly byte[] s_universityOfUtf8 = Encoding.UTF8.GetBytes("University of");

        public async Task<Byte[]> ExportJsonToExcel(string fileName, JsonDocument jsonDocument)
        {
            fileName = fileName + ".xlsx";
            var jsonArray = new JsonElement();

            if (jsonDocument.RootElement.ValueKind != JsonValueKind.Array)
            {
                //Get the root object and enumerate
                var properties = jsonDocument.RootElement.EnumerateObject();
                //Take next. We should get array object there
                properties.MoveNext();
                //Get the value of array
                jsonArray = properties.Current.Value;
            }
            else
            {
                jsonArray = jsonDocument.RootElement;
            }
            
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("0");

                HeadSheet(ref worksheet, jsonArray);

                FillSheet(ref worksheet, jsonArray);

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
        }

        private void HeadSheet(ref IXLWorksheet worksheet, JsonElement jsonArray)
        {
            int index = 1;

            foreach (JsonElement element in jsonArray.EnumerateArray())
            {
                foreach (JsonProperty property in element.EnumerateObject())
                {
                    if (property.Value.ValueKind == JsonValueKind.Array) continue;
                    if (property.Value.ValueKind == JsonValueKind.Object) continue;
                    worksheet.Cell(1, index).Style.Font.Bold = true;
                    worksheet.Cell(1, index).Value = property.Name;

                    index++;
                } 
                break;
            }
        }

        private void FillSheet(ref IXLWorksheet worksheet, JsonElement jsonArray)
        {
            List<String> headDictionary = new List<String>();

            int rowIndex = 2;
            int colIndex = 1;
            double dbl;

            foreach (JsonElement element in jsonArray.EnumerateArray())
            {
                foreach (JsonProperty property in element.EnumerateObject())
                {
                    //If Json object is array or object skip them
                    if (property.Value.ValueKind == JsonValueKind.Array) continue; 
                    if (property.Value.ValueKind == JsonValueKind.Object) continue;
                    //Cause of excel representation numbers we must add ' at the start of numbers longer then 11 digits
                    //So we check all the objects because most of number values have string type
                    if (double.TryParse(property.Value.ToString(), out dbl))
                    {
                        worksheet.Cell(rowIndex, colIndex).Value = "'"+ property.Value;
                    }
                    else
                    {
                        worksheet.Cell(rowIndex, colIndex).Value = property.Value;
                    }
                    //Ternary isn't good enough there
                    // worksheet.Cell(rowIndex, colIndex).Value = property.Value.ValueKind == JsonValueKind.Number ? "'"+ property.Value : property.Value;

                    colIndex++;
                }
                colIndex = 1; 
                rowIndex++;
            }
        }

        public async Task TestJson()
        {
             // The JSON data used for the samples was borrowed from https://github.com/Hipo/university-domains-list
            // under the MIT License (MIT).

            // string outputMessage = SyncFileExample("world_universities_and_domains.json");
            // Console.WriteLine("Reading JSON from file, sync: " + outputMessage);

            string outputMessage = await AsyncWebExample(@"http://universities.hipolabs.com/search?country=United%20States");
            Console.WriteLine("Reading JSON from web, async: " + outputMessage);
            outputMessage = await AsyncWebExample(@"http://universities.hipolabs.com/search?", worldWide: true);
            Console.WriteLine("Reading JSON from web, async: " + outputMessage);
        }

        public async Task ParseJson(JsonDocument jD)
        {
            string returnStr = "";

            var properties = jD.RootElement.EnumerateObject();
            properties.MoveNext();
            var curre = properties.Current;
            {
                foreach (JsonProperty property in jD.RootElement.EnumerateObject())
                {
                    string key = property.Name;
                    JsonElement valueElement = property.Value;
                    if (valueElement.ValueKind == JsonValueKind.Array)
                    {
                        // DeserializeArray(valueElement);
                    }
                    Console.WriteLine(key + " " + valueElement.ValueKind);
                    // string person_id = valueElement.GetProperty("person_id").GetString();
                    // string last_name = valueElement.GetProperty("last_name").GetString();
                    // string first_name = valueElement.GetProperty("first_name").GetString();
                }
                // JsonElement root = jD.RootElement;
                // foreach (JsonProperty property in root.ValueKind)
                // {
                //     if property.
                //     Console.WriteLine(JsonProperty.)
                //     var tempElem = property.Value;
                //     returnStr = returnStr + tempElem;
                // }
            }
        }

        private void DeserializeArray(JsonElement jsonArray)
        {
            Dictionary<string, string> headDictionary = new Dictionary<string, string>();
            foreach (JsonElement element in jsonArray.EnumerateArray())
            {
                foreach (JsonProperty property in element.EnumerateObject())
                {
                headDictionary.Add(property.Name, property.Value.ToString());
                } 
            }
        }


        ////////////////////////source code

        private static string SyncFileExample(string fileName)
        {
            // Follow the async web example if you want to read asynchronously from a FileStream instead.

            ReadOnlySpan<byte> dataWorld = GetUtf8JsonFromDisk(fileName);
            (int count, int total) = CountUniversityOf(dataWorld);
            double ratio = (double)count / total;
            return $"{count} out of {total} universities worldwide have names starting with 'University of' (i.e. {ratio.ToString("#.##%")})!";
        }

        private static ReadOnlySpan<byte> GetUtf8JsonFromDisk(string fileName)
        {
            // Read as UTF-16 and transcode to UTF-8 to return as a Span<byte>
            // For example:
            // string jsonString = File.ReadAllText(fileName);
            // return Encoding.UTF8.GetBytes(jsonString);

            // OR ReadAllBytes if the file encoding is known to be UTF-8 and skip the encoding step:
            byte[] jsonBytes = File.ReadAllBytes(fileName);
            return jsonBytes;
        }

        public static (int count, int total) CountUniversityOf(ReadOnlySpan<byte> dataUtf8)
        {
            int count = 0;
            int total = 0;

            var json = new Utf8JsonReader(dataUtf8, isFinalBlock: true, state: default);

            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;

                switch (tokenType)
                {
                    case JsonTokenType.StartObject:
                        total++;
                        break;
                    case JsonTokenType.PropertyName:
                        if (json.ValueSpan.SequenceEqual(s_nameUtf8))
                        {
                            bool result = json.Read();

                            // Debug.Assert(result);  // Assume valid JSON
                            // Debug.Assert(json.TokenType == JsonTokenType.String);   // Assume known, valid JSON schema

                            if (json.ValueSpan.StartsWith(s_universityOfUtf8))
                            {
                                count++;
                            }
                        }
                        break;
                }
            }
            return (count, total);
        }

        private static async Task<string> AsyncWebExample(string url, bool worldWide = false)
        {
            using (var client = new HttpClient())
            {
                using (Stream stream = await client.GetStreamAsync(url))
                {
                    (int count, int total) = await ReadJsonFromStreamUsingSpan(stream);

                    double ratio = (double)count / total;
                    string percentage = ratio.ToString("#.##%");
                    string outputMessage = worldWide ?
                        $"{count} out of {total} universities worldwide have names starting with 'University of' (i.e. {percentage})!" :
                        $"{count} out of {total} American universities have names starting with 'University of' (i.e. {percentage})!";

                    return outputMessage;
                }
            }
        }

        public static async Task<(int count, int total)> ReadJsonFromStreamUsingSpan(Stream stream)
        {
            // Assumes all JSON strings in the payload are small (say < 500 bytes)
            var buffer = new byte[1_024];
            int count = 0;
            int total = 0;

            JsonReaderState state = default;
            int leftOver = 0;
            int partialCount = 0;
            int partialTotalCount = 0;
            bool foundName = false;

            while (true)
            {
                // The Memory<byte> ReadAsync overload returns ValueTask which is allocation-free
                // if the operation completes synchronously
                int dataLength = await stream.ReadAsync(buffer.AsMemory(leftOver, buffer.Length - leftOver));
                int dataSize = dataLength + leftOver;
                bool isFinalBlock = dataSize == 0;
                long bytesConsumed = 0;

                (bytesConsumed, partialCount, partialTotalCount) = PartialCountUniversityOf(buffer.AsSpan(0, dataSize), isFinalBlock, ref foundName, ref state);

                // Based on your scenario and input data, you may need to grow your buffer here
                // It's possible that leftOver == dataSize (if a JSON token is too large)
                // so you need to resize and read more than 1_024 bytes.
                leftOver = dataSize - (int)bytesConsumed;
                if (leftOver != 0)
                {
                    buffer.AsSpan(dataSize - leftOver, leftOver).CopyTo(buffer);
                }

                count += partialCount;
                total += partialTotalCount;

                if (isFinalBlock)
                {
                    break;
                }
            }

            return (count, total);
        }

        public static (long bytesConsumed, int count, int total) PartialCountUniversityOf(ReadOnlySpan<byte> dataUtf8, bool isFinalBlock, ref bool foundName, ref JsonReaderState state)
        {
            int count = 0;
            int total = 0;

            var json = new Utf8JsonReader(dataUtf8, isFinalBlock, state);

            while (json.Read())
            {
                JsonTokenType tokenType = json.TokenType;

                switch (tokenType)
                {
                    case JsonTokenType.StartObject:
                        total++;
                        break;
                    case JsonTokenType.PropertyName:
                        if (json.ValueSpan.SequenceEqual(s_nameUtf8))
                        {
                            foundName = true;
                        }
                        break;
                    case JsonTokenType.String:
                        if (foundName && json.ValueSpan.StartsWith(s_universityOfUtf8))
                        {
                            count++;
                        }
                        foundName = false;
                        break;
                }
            }

            state = json.CurrentState;
            return (json.BytesConsumed, count, total);
        }
    }
}