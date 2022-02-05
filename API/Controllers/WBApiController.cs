using API.DTOs;
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

        public WBApiController(IWBService WBService)
        {
            _WBService = WBService;
        }

        

        // [HttpPost("GetOrders")]

        // public async Task<ActionResult> Getorders(OrdersDto ordersDto)
        // {
            // List of params from Swagger WB Api: 
            //  date_start(required) - С какой даты вернуть сборочные задания (заказы) (в формате RFC3339)
            //  date_end             - По какую дату вернуть сборочные задания (заказы) (в формате RFC3339)
            //  status               - Заказы какого статуса нужны
            //  take(required)       - Сколько записей вернуть за раз
            //  skip(required)       - Сколько записей пропустить
            //  id                   - Идентификатор сборочного задания, если нужно получить данные по какому-то определенному заказу

        //     string apiUrl = string.Format("https://suppliers-api.wildberries.ru/api/v2/orders?date_start={0}&status={1}&take={2}&skip={3}",
        //         HttpUtility.UrlEncode(ordersDto.Date_start),
        //         "2",
        //         ordersDto.Take.ToString(),
        //         ordersDto.Skip.ToString()
        //         );   
        //     //string apiTest = "https://suppliers-api.wildberries.ru/api/v2/orders?date_start=2022-01-19T00%3A00%3A00%2B03%3A00&status=2&take=10&skip=0";
        //     List<Object> reservationList = new List<Object>();
        //     using (var httpClient = _WBService.getHttpClient2())
        //     {
        //         HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
        //         if (response.IsSuccessStatusCode)
        //         {
        //             var jsonString = response.Content.ReadAsStringAsync().Result;
        //             return Content(jsonString, "application/json");
        //         }
        //         else
        //             return BadRequest("Can not reach");
        //     }
        // }

        [HttpPost("GetOrders")]
        public async Task<ActionResult> GetOrders(OrdersDto ordersDto)
        {
            //List of params from Swagger WB Api: 
            //  date_start(required) - С какой даты вернуть сборочные задания (заказы) (в формате RFC3339)
            //  date_end             - По какую дату вернуть сборочные задания (заказы) (в формате RFC3339)
            //  status               - Заказы какого статуса нужны
            //  take(required)       - Сколько записей вернуть за раз
            //  skip(required)       - Сколько записей пропустить
            //  id                   - Идентификатор сборочного задания, если нужно получить данные по какому-то определенному заказу
            string apiUrl = "https://suppliers-api.wildberries.ru/api/v2/orders";
            apiUrl = _WBService.GetFullUrl(apiUrl, ordersDto);
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

        [HttpGet("GetSales/{date}")]
        [Authorize]
        public async Task<ActionResult> GetSales(string dateStart)
        {
            var todayString = "2022-01-17T00%3A00%3A00%2B03%3A00";
            List<Object> reservationList = new List<Object>();
            using (var httpClient = _WBService.GetHttpClient2())
            {
                HttpResponseMessage response = await httpClient.GetAsync("https://suppliers-api.wildberries.ru/api/v2/orders?date_start=" + todayString + "&status=2&take=1&skip=0");
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    return Content(jsonString, "application/json");
                }
                else
                    return BadRequest("Can not reach");
            }
        }

    }
}