using API.Data;
using API.DTOs;
using API.Entitites.WBEntities;
using API.Helpers;
using API.Interfaces;
using API.Services;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;

namespace API.Controllers
{
    [Authorize]
    public class WBApiController : BaseApiController
    {
        private HttpClient httpClient;
        private readonly IWBService _WBService;
        private readonly DataContext _context;

        public WBApiController(IWBService WBService, DataContext context)
        {
            _WBService = WBService;
            _context = context;
        }

        [HttpPost("ExportToExcel")]
        public async Task<ActionResult> ExportToExcel([FromBody] JsonDocument context)
        {
            var eh = new ExcelHelper();
            string filename = "Excel";
            // await eh.ExportJsonToExcel(filename, context);
            var content = await eh.ExportJsonToExcel(filename, context);

             return File(
                content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "users.xlsx");
            // return BadRequest();
        }

        [HttpPost("GetSales")]
        [Authorize]
        public async Task<ActionResult> GetSales(SalesDto salesDto)
        {
            string apiUrl = "https://suppliers-stats.wildberries.ru/api/v1/supplier/sales";
            apiUrl = _WBService.GetFullUrl(apiUrl, salesDto, true);

            List<Object> reservationList = new List<Object>();
            using (var httpClient = _WBService.GetHttpClientOld())
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    UploadSalesToDB(jsonString);
                    return Content(jsonString, "application/json");
                }
                else
                    return BadRequest("Can not reach");
            }
        }

        [HttpPost("GetOrders")]
        [Authorize]
        public async Task<ActionResult> GetOrders(OrdersDto ordersDto)
        {
            string apiUrl = "https://suppliers-stats.wildberries.ru/api/v1/supplier/orders";
            apiUrl = _WBService.GetFullUrl(apiUrl, ordersDto, true);

            List<Object> reservationList = new List<Object>();
            using (var httpClient = _WBService.GetHttpClientOld())
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    UploadOrdersToDB(jsonString);
                    return Content(jsonString, "application/json");
                }
                else
                    return BadRequest("Can not reach");
            }
        }

        [HttpPost("GetFBSOrders")]
        public async Task<ActionResult> GetFBSOrders(FBSOrdersDto FBSOrdersDto)
        {
            //List of params from Swagger WB Api: 
            //  date_start(required) - С какой даты вернуть сборочные задания (заказы) (в формате RFC3339)
            //  date_end             - По какую дату вернуть сборочные задания (заказы) (в формате RFC3339)
            //  status               - Заказы какого статуса нужны
            //  take(required)       - Сколько записей вернуть за раз
            //  skip(required)       - Сколько записей пропустить
            //  id                   - Идентификатор сборочного задания, если нужно получить данные по какому-то определенному заказу
            string apiUrl = "https://suppliers-api.wildberries.ru/api/v2/orders";
            apiUrl = _WBService.GetFullUrl(apiUrl, FBSOrdersDto);
            // return Content(apiUrl);
            using (var httpClient = _WBService.GetHttpClient2())
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    return Content(jsonString, "application/json");
                }
                else
                    return BadRequest("Can not reach");
            }
        }

        private void UploadSalesToDB(string jsonString)
        {
            List<Sale> objData = JsonSerializer.Deserialize<List<Sale>>(jsonString);
            _context.Sales.AddRangeAsync(objData);
            _context.SaveChanges();
        }

          private void UploadOrdersToDB(string jsonString)
        {
            List<Order> objData = JsonSerializer.Deserialize<List<Order>>(jsonString);
            _context.Orders.AddRange(objData);
            try
            {
                 _context.SaveChanges();
            }
            catch
            {

            }
        }

    }
}