using API.DTOs;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
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

        [HttpPost("GetOrders")]

        public async Task<ActionResult> Getorders(OrdersDto ordersDto)
        {
            string apiUrl = string.Format("https://suppliers-api.wildberries.ru/api/v2/orders?date_start={0}&status={1}&take={2}&skip={3}",
                HttpUtility.UrlEncode(ordersDto.Date_start),
                "2",
                ordersDto.Take.ToString(),
                ordersDto.Skip.ToString()
                );
            //string apiTest = "https://suppliers-api.wildberries.ru/api/v2/orders?date_start=2022-01-19T00%3A00%3A00%2B03%3A00&status=2&take=10&skip=0";
            List<Object> reservationList = new List<Object>();
            using (var httpClient = _WBService.getHttpClient2())
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

        [HttpGet("GetSales/{date}")]
        [Authorize]
        public async Task<ActionResult> GetSales(string dateStart)
        {
            var todayString = "2022-01-17T00%3A00%3A00%2B03%3A00";
            List<Object> reservationList = new List<Object>();
            using (var httpClient = _WBService.getHttpClient2())
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