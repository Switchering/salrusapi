using System.Text.Json;
using API.Data;
using API.DTOs;
using API.Entitites.WBEntities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class SalrusApiController : BaseApiController
    {
        private HttpClient httpClient;
        private readonly DataContext _context;
        private readonly ISalrusService _salrusService;

        public SalrusApiController(DataContext context, ISalrusService salrusService)
        {
            _context = context;
            _salrusService = salrusService;
        }

        [HttpPost("GetOrders")]
        [Authorize]
        public async Task<ActionResult> GetOrders(OrdersDto ordersDto)
        {
            List<Order> orders = _context.Orders.Take(100).ToList();
            string jsonString = JsonSerializer.Serialize(orders);
            using (var httpClient = _salrusService.GetHttpClient())
            {
                return Content(jsonString, "application/json");
                
                return BadRequest("Can not reach");
            }
        }
    }
} 
