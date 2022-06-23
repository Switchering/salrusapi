using System.Text.Json;
using API.Data;
using API.DTOs;
using API.Entitites.WBEntities;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Helpers;
using API.Entitites.PrintEntities;

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
        public async Task<ActionResult> GetOrders(OrdersDto ordersDto)
        {
            List<Order> orders = await _context.Orders.Take(100).ToListAsync();
            string jsonString = JsonSerializer.Serialize(orders);
            using (var httpClient = _salrusService.GetHttpClient())
            {
                return Content(jsonString, "application/json");
            }
        }

        ///////////////////////////////////////////////////////////////////////
        //Print api

        [HttpPost("UploadFBS")]

        public async Task<ActionResult> UploadFBS([FromForm]IFormFile file)
        {
            if (file == null || file.Length == 0)
            return Content("File Not Selected");

            string fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
                return Content("File Is Not Excel");

            ExcelHelper eh = new ExcelHelper();

            PrintOrder printOrder = eh.GetPrintOrder(file.FileName, "FBS");
            if (OrderExist(printOrder.Order_Id).Result)
            {
                return Ok("Order alredy exist!");
            };
            Stream stream = file.OpenReadStream();
            List<FBSProduct> DataList = eh.GetFBSProducts(stream, printOrder.Order_Id);
            
            _context.PrintOrders.Add(printOrder);
            _context.FBS.AddRange(DataList);
            _context.SaveChanges();

            return Ok("Order uploaded");
        }

        [HttpPost("UploadFBO")]
        public async Task<ActionResult> UploadFBO([FromForm]IFormFile file)
        {
            if (file == null || file.Length == 0)
            return Content("File Not Selected");

            string fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
                return Content("File Is Not Excel");

            ExcelHelper eh = new ExcelHelper();

            PrintOrder printOrder = eh.GetPrintOrder(file.FileName, "FBO");
            if (OrderExist(printOrder.Order_Id).Result)
            {
                return Ok("Order alredy exist!");
            };
            Stream stream = file.OpenReadStream();
            List<FBSProduct> DataList = eh.GetFBSProducts(stream, printOrder.Order_Id);
            
            _context.PrintOrders.Add(printOrder);
            _context.FBS.AddRange(DataList);
            _context.SaveChanges();

            return Ok("Order uploaded");
        }

        [HttpPost("UpdateFBS")]
        public async Task<ActionResult> UpdateFBS([FromForm]IFormFile file)
        {
            if (file == null || file.Length == 0)
            return Content("File Not Selected");

            string fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
                return Content("File Is Not Excel");

            ExcelHelper eh = new ExcelHelper();

            PrintOrder printOrder = eh.GetPrintOrder(file.FileName, "FBS");

            Stream stream = file.OpenReadStream();
            List<FBSProduct> DataList = eh.GetFBSProducts(stream, printOrder.Order_Id);
            
            List<FBSProduct> dbList = _context.FBS.Where(p => p.Order_Id == printOrder.Order_Id && p.Printed == "New").ToList();

            

            _context.FBS.AddRange(DataList);
            await _context.SaveChangesAsync();

            return Ok("Order uploaded");
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

        private async Task<bool> OrderExist(string order_Id)
        {
            return await _context.PrintOrders.AnyAsync(x => x.Order_Id == order_Id);
        }
    }
} 
