using API.Data;
using API.DTOs;
using API.Entitites.WBEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.JDOs;
using API.Helpers;
using System.Text.Json;
using AutoMapper;
using API.Interfaces;

namespace API.Controllers
{
    public class TestController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IWBRepository _wBRepository;
        public TestController(DataContext context, IMapper mapper, IWBRepository wBRepository)
        {
            _wBRepository = wBRepository;
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("IncomeDuplicates")]
        [Authorize]
        public async Task<ActionResult> IncomeDuplicates(IncomesDto incomesDto)
        {
            var duplicates = _context.IncomeDetails.GroupBy(i => i.barcode)
                     .Where(x => x.Count() > 1)
                     .Select(val => val.Key);            

            foreach (var inc in duplicates)
            {

            }
            return BadRequest("Can not reach");
        }

        [HttpPost("quarterer")]
        public ActionResult quarterer()
        {
            string jsonString;
            List<JSONOrder> orderList;
            using (StreamReader r = new StreamReader("TestData/TestOrders.json"))
            {
                jsonString = r.ReadToEnd();
            }
            if (jsonString is null)
                return BadRequest();
            orderList = JsonSerializer.Deserialize<List<JSONOrder>>(jsonString);
            List<Type> types = new List<Type>() { typeof(Order), typeof(OrderDetail) };

            Quarterer<JSONOrder> quarterer = new Quarterer<JSONOrder>(orderList, types);
            var quartedLists = quarterer.Quarter();
            return Ok();
        }

        [HttpPost("MapTest")]
        public ActionResult MapTest()
        {
            string jsonString;
            List<JSONOrder> JSONOrderList;
            using (StreamReader r = new StreamReader("TestData/TestOrders.json"))
            {
                jsonString = r.ReadToEnd();
            }
            if (jsonString is null)
                return BadRequest();
            JSONOrderList = JsonSerializer.Deserialize<List<JSONOrder>>(jsonString);
            var orderList = new List<Order>();
            foreach (JSONOrder jdo in JSONOrderList)
            {
                Order order = _mapper.Map<Order>(jdo);
                orderList.Add(order);
            }
            string resultString = JsonSerializer.Serialize(orderList);
            return Content(resultString, "application/json");
        }

        [HttpPost("GetOrders")]
        [Authorize]
        public async Task<ActionResult> GetOrders(OrdersDto ordersDto)
        {
            string jsonString;
            List<JSONOrder> JSONOrderList;

            using (StreamReader r = new StreamReader("TestData/orders.json"))
            {
                jsonString = r.ReadToEnd();
            }
            if (jsonString is null)
                return BadRequest();
            JSONOrderList = JsonSerializer.Deserialize<List<JSONOrder>>(jsonString);
    
            await _wBRepository.UploadOrdersToDB(JSONOrderList, ordersDto);

            return Content(jsonString, "application/json");
        
        }

         [HttpPost("GetSales")]
        [Authorize]
        public async Task<ActionResult> GetSales(SalesDto salesDto)
        {
            string jsonString;
            List<JSONSale> JSONOSaleList;
            using (StreamReader r = new StreamReader("TestData/sales.json"))
            {
                jsonString = r.ReadToEnd();
            }
            if (jsonString is null)
                return BadRequest();
            JSONOSaleList = JsonSerializer.Deserialize<List<JSONSale>>(jsonString);
            
            await _wBRepository.UploadSalesToDB(JSONOSaleList, salesDto);

            return Content(jsonString, "application/json");
        
        }
    }
}