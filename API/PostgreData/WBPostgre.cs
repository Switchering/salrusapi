using System.Text.Json;
using API.Data;
using API.Entitites.WBEntities;
using API.JDOs;

namespace PostgreData
{
    public class WBPostgre
    {
        private readonly DataContext _context;

        public WBPostgre(DataContext context)
        {
            _context = context;
        }

        public async Task UploadIncomesToDB(string jsonString)
        {
            //Имеющиейся в базе поставки
            var incomeIdList = _context.Incomes.Select(p => p.incomeId).ToList();
            //Выгруженные с апи вб данные 
            List<JSONIncome> objData = JsonSerializer.Deserialize<List<JSONIncome>>(jsonString);
            //Фильтруем данные от уже имеющихся в базе
            foreach (var item in incomeIdList)
            {
                objData.RemoveAll(o => (o.incomeId == item & o.dateClose != DateTime.MinValue));
            }
            //Создаем списки поставки и состав поставки
            List<Income> incomeList = new List<Income>();
            List<IncomeDetail> incomeDetailsList =  new List<IncomeDetail>();

            //Разделяем данные с апи на сущности поставки и состава
            foreach (JSONIncome objItem in objData)
            {
                Income incomeItem = new Income();
                IncomeDetail incomeDetailsItem = new IncomeDetail();

                foreach (var prop in objItem.GetType().GetProperties())
                {
                    var tempProp = incomeItem.GetType().GetProperty(prop.Name);
                    if (tempProp != null)
                    {
                        tempProp.SetValue(incomeItem, prop.GetValue(objItem));
                    }

                    tempProp = incomeDetailsItem.GetType().GetProperty(prop.Name);
                    if (tempProp != null)
                    {
                        tempProp.SetValue(incomeDetailsItem, prop.GetValue(objItem));
                    }
                }

                if (!incomeIdList.Exists(x => x == incomeItem.incomeId) & !incomeList.Exists(x => x .incomeId == incomeItem.incomeId)) 
                    incomeList.Add(incomeItem);
                
                incomeDetailsList.Add(incomeDetailsItem);   
            }
            _context.AddRangeAsync(incomeList);
            
            var statusList = _context.IncomeDetails.Where(p => p.status == "Приемка").ToList();

            foreach (var item in statusList)
            {
                IncomeDetail temp = incomeDetailsList.Find(o => (o.barcode == item.barcode & o.incomeId == item.incomeId & o.quantity == item.quantity));
                item.status = temp.status;
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

        public async Task UploadOrdersToDB(string jsonString)
        {
            //Получаем первый и последний заказы
            var lastOrder = _context.Orders.OrderByDescending(o => o.odid).FirstOrDefault();
            var firstOrder = _context.Orders.OrderBy(o => o.odid).FirstOrDefault();
        
            //фильтруем выгруженные заказы 
            List<Order> objData = JsonSerializer.Deserialize<List<Order>>(jsonString);
            if (firstOrder is not null)
            {
                objData.RemoveAll(o => (o.odid >= firstOrder.odid & o.odid <= lastOrder.odid));
            }
            //Добавляем список в модель и выгружаем в базу
            _context.AddRangeAsync(objData);
            _context.SaveChanges();
        }
    }
}