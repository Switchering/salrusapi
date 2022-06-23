using System.Text;
using System.Text.Json;
using ClosedXML.Excel;
using API.Entitites.PrintEntities;

namespace API.Helpers
{
    //Class export Json to excel
    public class ExcelHelper
    {

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

        public PrintOrder GetPrintOrder(string fileName, string orderType)
        {
            if (orderType == "FBS")
            {
                int startIndex = fileName.IndexOf("(API");
                string number = fileName.Substring(startIndex+4);
                int endIndex = number.IndexOf(")");
                number = number.Substring(0, endIndex);
                PrintOrder printOrder = new PrintOrder{
                    Order_Date = DateTime.Now,
                    Order_Id = number,
                    Order_Type = orderType
                };
                return printOrder;
            }
            else
            {
                int startIndex = fileName.IndexOf("(API");
                string number = fileName.Substring(startIndex+4);
                int endIndex = number.IndexOf(")");
                number = number.Substring(0, endIndex);
                PrintOrder printOrder = new PrintOrder{
                    Order_Date = DateTime.Now,
                    Order_Id = number,
                    Order_Type = orderType
                };
                return printOrder;
            }  
        }

        public List<FBSProduct> GetFBSProducts(Stream stream, string order_Id)
        {
            
            using (XLWorkbook workbook = new XLWorkbook(stream))
            {
                IXLWorksheet workSheet = workbook.Worksheet(1);
                //var workSheet = package.Workbook.Worksheets.First();
                
                int totalRows = workSheet.RowCount();

                var DataList = new List<FBSProduct>();

                for (int i = 2; i <= totalRows; i++)
                {
                    if (workSheet.Cell(i, 6).Value.ToString() == "")
                        continue;

                    DataList.Add(new FBSProduct
                        {
                            CodSalrus = workSheet.Cell(i, 1).Value.ToString(),        
                            Quant = Convert.ToInt32(workSheet.Cell(i, 2).Value),
                            Naim = workSheet.Cell(i, 3).Value.ToString(),
                            Art = workSheet.Cell(i, 4).Value.ToString(),
                            Art_Color = workSheet.Cell(i, 5).Value.ToString(),
                            SHKWB = workSheet.Cell(i, 6).Value.ToString(),
                            Sticker1 = workSheet.Cell(i, 7).Value.ToString(),
                            Sticker2 = workSheet.Cell(i, 8).Value.ToString(),
                            Request = workSheet.Cell(i, 9).Value.ToString(),
                            SHK1 = workSheet.Cell(i, 10).Value.ToString(),
                            SHK2 = workSheet.Cell(i, 11).Value.ToString(),
                            SHK3 = workSheet.Cell(i, 12).Value.ToString(),
                            SHK1C = workSheet.Cell(i, 13).Value.ToString(),
                            Printed = "New",
                            Order_Id = order_Id
                        });
                }

                return DataList;
            }
        }

        public List<FBSProduct> GetFBOProducts(Stream stream, string order_Id)
        {
            
            using (XLWorkbook workbook = new XLWorkbook(stream))
            {
                IXLWorksheet workSheet = workbook.Worksheet(1);
                //var workSheet = package.Workbook.Worksheets.First();
                
                int totalRows = workSheet.RowCount();

                var DataList = new List<FBSProduct>();

                for (int i = 2; i <= totalRows; i++)
                {
                    if (workSheet.Cell(i, 6).Value.ToString() == "")
                        continue;

                    DataList.Add(new FBSProduct
                        {
                            CodSalrus = workSheet.Cell(i, 1).Value.ToString(),        
                            Quant = Convert.ToInt32(workSheet.Cell(i, 2).Value),
                            Naim = workSheet.Cell(i, 3).Value.ToString(),
                            Art = workSheet.Cell(i, 4).Value.ToString(),
                            Art_Color = workSheet.Cell(i, 5).Value.ToString(),
                            SHKWB = workSheet.Cell(i, 6).Value.ToString(),
                            Sticker1 = workSheet.Cell(i, 7).Value.ToString(),
                            Sticker2 = workSheet.Cell(i, 8).Value.ToString(),
                            Request = workSheet.Cell(i, 9).Value.ToString(),
                            SHK1 = workSheet.Cell(i, 10).Value.ToString(),
                            SHK2 = workSheet.Cell(i, 11).Value.ToString(),
                            SHK3 = workSheet.Cell(i, 12).Value.ToString(),
                            SHK1C = workSheet.Cell(i, 13).Value.ToString(),
                            Printed = "New",
                            Order_Id = order_Id
                        });
                }

                return DataList;
            }
        }
   }
}