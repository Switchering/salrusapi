using API.Data;
using API.DTOs;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostgreData;
using System.Text.Json;


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

        //Excel functions move to salrusApi
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

        //Rename to UploadSales etc
        [HttpPost("GetSales")]
        [Authorize]
        public async Task<ActionResult> GetSales(SalesDto salesDto)
        {
            string apiUrl = "https://suppliers-stats.wildberries.ru/api/v1/supplier/sales";
            apiUrl = _WBService.GetFullUrl(apiUrl, salesDto, true);

            using (var httpClient = _WBService.GetHttpClientOld())
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    //UploadSalesToDB(jsonString);
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

            using (var httpClient = _WBService.GetHttpClientOld())
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;

                    WBPostgre wbp = new WBPostgre(_context);
                    await wbp.UploadOrdersToDB(jsonString);

                    return Content(jsonString, "application/json");
                }
                else
                    return BadRequest("Can not reach");
            }
        }

        [HttpPost("GetIncomes")]
        [Authorize]
        public async Task<ActionResult> GetIncomes(IncomesDto incomesDto)
        {
            string apiUrl = "https://suppliers-stats.wildberries.ru/api/v1/supplier/incomes";
            apiUrl = _WBService.GetFullUrl(apiUrl, incomesDto, true);

            using (var httpClient = _WBService.GetHttpClientOld())
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;

                    WBPostgre wbp = new WBPostgre(_context);
                    await wbp.UploadIncomesToDB(jsonString);

                    return Content(jsonString, "application/json");
                }
                else
                    return BadRequest("Can not reach");
            }
        }



        //WB Api 2.0
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
    }
}