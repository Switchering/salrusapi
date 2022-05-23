using System.Text.Json;
using API.Data;
using API.DTOs;
using API.Entitites.WBEntities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class SalrusApiController : BaseApiController
    {
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
            List<Order> orders = await _context.Orders.Take(100).ToListAsync();
            string jsonString = JsonSerializer.Serialize(orders);
            using (var httpClient = _salrusService.GetHttpClient())
            {
                return Content(jsonString, "application/json");
            }
        }

        [HttpPost("UploadFBO")]
        [Authorize]
        public async Task<ActionResult> UploadFBO(OrdersDto ordersDto)
        {
            List<Order> orders = await _context.Orders.Take(100).ToListAsync();
            string jsonString = JsonSerializer.Serialize(orders);
            using (var httpClient = _salrusService.GetHttpClient())
            {
                return Content(jsonString, "application/json");
            }
        }
    }
} 
