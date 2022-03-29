using System.Net;
using System.Text;
using System.Text.Json;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;

namespace API.Controllers
{
    public class ParseController : BaseApiController
    {
        private HttpClient httpClient;

        string wbUrl =  "https://www.wildberries.ru/catalog/0/search.aspx?page=1&sort=popular&search=%D1%81%D1%82%D0%B5%D0%BB%D1%8C%D0%BA%D0%B8";
        string testUrl = "http://books.toscrape.com/catalogue/category/books/mystery_3/index.html";

        public ParseController()
        {
            
        }
        
        [HttpGet("Test")]
        public IActionResult Test()
        {
            var result = GetBookLinks(wbUrl);
            var content = JsonSerializer.Serialize(result);
            return Content(content);
        }

        static async Task<List<string>> GetBookLinks(string url)
        {
            var cardLinks = new Dictionary<int,string>();
            var sellerNames = new List<string>();
            int i = 0;
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false
            });
            var page = await browser.NewPageAsync();
            await page.SetRequestInterceptionAsync(true);

            // disable images to download
            page.Request += (sender, e) =>
            {
                if (e.Request.ResourceType == ResourceType.Image)
                    e.Request.AbortAsync();
                else
                    e.Request.ContinueAsync();
            };
            await page.GoToAsync(url);
            await page.WaitForSelectorAsync("div.product-card__wrapper");

            var links = @"Array.from(document.querySelectorAll('.product-card__wrapper')).map(a => a.children[0].href);";
            var urls = await page.EvaluateExpressionAsync<string[]>(links);


            foreach (string item in urls)
            {
                cardLinks.Add(i,item);
                i++;
            }

            HtmlDocument doc = new HtmlDocument();
            foreach (var item in cardLinks)
            {
                await page.GoToAsync(item.Value);
                await page.WaitForSelectorAsync(".seller-details__title");
                var html = await page.GetContentAsync();

                doc.LoadHtml(html);

                HtmlNode sellerName = doc.DocumentNode.SelectSingleNode(".//a[contains(@class, 'seller-details__title')]");
                string name = sellerName.InnerText;
                sellerNames.Add(name);
            }

            return sellerNames;

            
            // HtmlNodeCollection linkNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'product-card__wrapper')]");

            // foreach (HtmlNode div in linkNodes)
            // {
            //     Console.WriteLine(div.InnerHtml);
            //     HtmlNode priceNode = div.SelectSingleNode(".//ins[contains(@class, 'lower-price')]");
            //     string price = priceNode.InnerText;
            //     HtmlNode titleNode = div.SelectSingleNode(".//span[contains(@class, 'goods-name')]");
            //     string title = titleNode.InnerText;
            //     string fullStr = title +" costs: " + price;
            //     bookLinks.Add(fullStr);
            // }
            // return bookLinks;

            // HtmlDocument doc = GetDocument(url);
            // // HtmlNode seller = doc.DocumentNode.SelectSingleNode("//a[contains(@class, 'seller-details__title']");
            //  foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            //     {
            //         bookLinks.Add(link.Attributes["href"].Value);
            //     }
            // // bookLinks.Add(seller.Attributes["href"].Value);
            // return bookLinks;
        }

        // private async Task<> 

        static HtmlDocument GetDocument(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            return doc;
        }
    }
}