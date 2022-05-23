using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entitites.WBEntities;
using API.JDOs;
using API.DTOs;
using API.Helpers;
using AutoMapper;

namespace PostgreData
{
    public class WBPostgre
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public WBPostgre(DataContext context)
        {
            _context = context;
        }

        public async Task UploadIncomesToDB(string jsonString, IncomesDto ordersDto)
        {
            DateTime fromDate = DateTime.Parse(ordersDto.dateFrom);
            //Имеющиейся в базе поставки
            var contextIncomeList = _context.Incomes.Where(p => p.date >= fromDate).ToList();
            //Выгруженные с апи вб данные 
            List<JSONIncome> objData = JsonSerializer.Deserialize<List<JSONIncome>>(jsonString);
            //Фильтруем данные от уже имеющихся в базе
            foreach (var item in contextIncomeList)
            {
                objData.RemoveAll(o => (o.incomeId == item.incomeId & o.dateClose == item.dateClose));
            }
            //Создаем списки поставки и состав поставки
            List<Income> incomeList = new List<Income>();
            List<IncomeDetail> incomeDetailsList =  new List<IncomeDetail>();

            //Разделяем данные с апи на сущности поставки и состава

            foreach (JSONIncome jdo in objData)
            {
                Income income = _mapper.Map<Income>(jdo);
                IncomeDetail incomeDetail = _mapper.Map<IncomeDetail>(jdo);
                incomeList.Add(income);
                incomeDetailsList.Add(incomeDetail);
            }

            // foreach (JSONIncome objItem in objData)
            // {
            //     Income incomeItem = new Income();
            //     IncomeDetail incomeDetailsItem = new IncomeDetail();

            //     foreach (var prop in objItem.GetType().GetProperties())
            //     {
            //         var tempProp = incomeItem.GetType().GetProperty(prop.Name);
            //         if (tempProp != null)
            //         {
            //             tempProp.SetValue(incomeItem, prop.GetValue(objItem));
            //         }

            //         tempProp = incomeDetailsItem.GetType().GetProperty(prop.Name);
            //         if (tempProp != null)
            //         {
            //             tempProp.SetValue(incomeDetailsItem, prop.GetValue(objItem));
            //         }
            //     }

            //     if (!contextIncomeList.Exists(x => x.incomeId == incomeItem.incomeId) & !incomeList.Exists(x => x.incomeId == incomeItem.incomeId)) 
            //         incomeList.Add(incomeItem);
                
            //     incomeDetailsList.Add(incomeDetailsItem);   
            // }

            foreach (Income item in incomeList)
            {
                Income temp = contextIncomeList.Find(o => o.incomeId == item.incomeId);
                if (temp is null) continue;
                temp.dateClose = item.dateClose;
                incomeList.Remove(temp);
            }
            _context.AddRangeAsync(incomeList);
            
            List<IncomeDetail> statusList = _context.IncomeDetails.Include(p => p.Income).Where(p => p.status != "Принято").ToList();

            foreach (IncomeDetail item in statusList)
            {
                IncomeDetail temp = incomeDetailsList.Find(o => (o.barcode == item.barcode & o.incomeId == item.incomeId));
                if (temp is null) continue;
                item.quantity = temp.quantity;
                item.status = temp.status;
                item.lastChangeDate = temp.lastChangeDate;
                incomeDetailsList.Remove(temp);
            }

            _context.AddRangeAsync(incomeDetailsList);
            _context.SaveChanges();
        }

        public async Task UploadSalesToDB(string jsonString)
        {
            List<Sale> objData = JsonSerializer.Deserialize<List<Sale>>(jsonString);
            _context.Sales.AddRangeAsync(objData);
            _context.SaveChanges();
        }

        public async Task UploadOrdersToDB(string jsonString, OrdersDto ordersDto)
        {
            //Получаем первый и последний заказы
            var lastOrder = _context.OrderDetails.OrderByDescending(o => o.odid).FirstOrDefault();
            var firstOrder = _context.OrderDetails.OrderBy(o => o.odid).FirstOrDefault();
        
            //фильтруем выгруженные заказы 
            List<JSONOrder> objData = JsonSerializer.Deserialize<List<JSONOrder>>(jsonString);
            if (firstOrder is not null)
            {
                objData.RemoveAll(o => (o.odid >= firstOrder.odid & o.odid <= lastOrder.odid));
            }
            //Добавляем список в модель и выгружаем в базу
            await _context.AddRangeAsync(objData);
            _context.SaveChanges();
        }
    }
}