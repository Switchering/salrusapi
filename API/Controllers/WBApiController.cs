using API.Data;
using API.DTOs;
using API.Helpers;
using API.Interfaces;
using API.JDOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace API.Controllers
{
    [Authorize]
    public class WBApiController : BaseApiController
    {
        private readonly IWBService _WBService;
        private readonly IMapper _mapper;
        private readonly IWBRepository _wBRepository;

        public WBApiController(IWBService WBService, IMapper mapper, IWBRepository wBRepository)
        {
            wBRepository = _wBRepository;
            _WBService = WBService;
            _mapper = mapper;
        }

        //Rename to UploadSales etc
        [HttpPost("GetSales")]
        public async Task<ActionResult> GetSales(SalesDto salesDto)
        {
            string apiUrl = "https://suppliers-stats.wildberries.ru/api/v1/supplier/sales";
            apiUrl = _WBService.GetFullUrl(apiUrl, salesDto, true);

            using (var httpClient = _WBService.GetHttpClientOld())
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = response.Content.ReadAsStringAsync().Result;
                    List<JSONSale> objData = JsonSerializer.Deserialize<List<JSONSale>>(jsonString);
                    objData = response.Content.ReadFromJsonAsync<List<JSONSale>>().Result;
                    if (objData.Count != 0)
                    {
                        await _wBRepository.UploadSalesToDB(objData, salesDto);
                    }
                    return Content("Загружено заказов: " + objData.Count.ToString());
                }
                else
                    return BadRequest(response.StatusCode);
            }
        }

        [HttpPost("GetOrders")]
        public async Task<ActionResult> GetOrders(OrdersDto ordersDto)
        {
            string apiUrl = "https://suppliers-stats.wildberries.ru/api/v1/supplier/orders";
            //Получаем URL с параметрами
            apiUrl = _WBService.GetFullUrl(apiUrl, ordersDto, true);

            using (var httpClient = _WBService.GetHttpClientOld())
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {                    
                    List<JSONOrder> objData = response.Content.ReadFromJsonAsync<List<JSONOrder>>().Result;     
                    if (objData.Count != 0)
                    {
                        await _wBRepository.UploadOrdersToDB(objData, ordersDto);
                    }
                    return Content("Загружено заказов: " + objData.Count.ToString());
                }
                else
                    return BadRequest(response.StatusCode);
            }
        }

        [HttpPost("GetIncomes")]
        public async Task<ActionResult> GetIncomes(IncomesDto incomesDto)
        {
            string apiUrl = "https://suppliers-stats.wildberries.ru/api/v1/supplier/incomes";
            apiUrl = _WBService.GetFullUrl(apiUrl, incomesDto, true);

            using (var httpClient = _WBService.GetHttpClientOld())
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    List<JSONIncome> objData = response.Content.ReadFromJsonAsync<List<JSONIncome>>().Result;     
                    //Дессериализация выгруженных данных
                    if (objData.Count != 0)
                    {
                        await _wBRepository.UploadIncomesToDB(objData, incomesDto);
                    }
                    return Content("Загружено поставок: " + objData.Count.ToString());
                }
                else
                    return BadRequest(response.StatusCode);
            }
        }

        [HttpPost("GetWBProducts")]
        public async Task<ActionResult> GetWBProducts()
        {
            string apiUrl = "https://suppliers-api.wildberries.ru/api/v2/card/list";
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