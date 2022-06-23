using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Entitites.WBEntities;
using API.JDOs;
using API.DTOs;
using API.Helpers;
using AutoMapper;
using API.Interfaces;

namespace API.Data
{
    public class WBRepository : IWBRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public WBRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        //Загрузка поставок WB в базу
        public async Task UploadIncomesToDB(List<JSONIncome> objData, IncomesDto incomesDto)
        {
            DateTime fromDate = DateTime.Parse(incomesDto.dateFrom);
            List<Income> modifiedIncomes = new List<Income>();

            foreach (JSONIncome jdo in objData)
            {
                if (!modifiedIncomes.Exists(x => x.incomeId == jdo.incomeId))
                {
                    Income income = _mapper.Map<Income>(jdo);
                    if (await _context.Incomes.AnyAsync(x => x.incomeId == income.incomeId))
                    {
                        _context.Incomes.Update(income);
                    }
                    else
                    {
                        _context.Incomes.Add(income);
                    }
                    modifiedIncomes.Add(income);
                }


                IncomeDetail incomeDetail = _mapper.Map<IncomeDetail>(jdo);
                IncomeDetail contextIncomeDetail = await _context.IncomeDetails.
                    FirstOrDefaultAsync(x => (x.incomeId == jdo.incomeId & x.barcode == jdo.barcode));
                if (contextIncomeDetail is not null)
                {
                    _context.IncomeDetails.Update(contextIncomeDetail);
                }
                else
                {
                    _context.IncomeDetails.Add(incomeDetail);
                }

                _context.SaveChanges();
            }
        }

        public async Task UploadSalesToDB(List<JSONSale> objData, SalesDto salesDto)
        {
            List<Sale> saleList = _mapper.Map<List<Sale>>(objData);
            List<ulong> errorList = new List<ulong>();
            foreach (Sale sale in saleList)
            {
                if (_context.Sales.Any(x => x.saleID == sale.saleID))
                    continue;
                try
                {
                    _context.Sales.AddAsync(sale);
                }
                catch
                {
                    if (sale.odid <= 0)
                    {
                        JSONSale tempSale = objData.First(x => x.saleID == sale.saleID);
                        _context.OrderDetails.Add(new OrderDetail
                        {
                            odid = tempSale.odid,
                            nmId = tempSale.nmId,
                            incomeId = tempSale.incomeID,
                            barcode = tempSale.barcode
                        });
                    }
                }
            }
            foreach (ulong strError in errorList)
            {
                Console.WriteLine(strError);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UploadOrdersToDB(List<JSONOrder> objData, OrdersDto ordersDto)
        {
            DateTime fromDate = DateTime.Parse(ordersDto.dateFrom);

            //Разделяем данные с апи на сущности заказа и состава
            foreach (JSONOrder jdo in objData)
            {
                //Если база содержит заказ пропускаем
                if (await _context.OrderDetails.AnyAsync(x => x.odid == jdo.odid))
                    continue;

                OrderDetail orderDetail = _mapper.Map<OrderDetail>(jdo);
                _context.OrderDetails.Add(orderDetail);

                ////Костыль нехватки первоначальных данных по поставкам
                if (!await _context.Incomes.AnyAsync(x => x.incomeId == jdo.incomeID))
                {
                    Income tempIncome = new Income
                    {
                        incomeId = jdo.incomeID,
                        date = DateTime.Now,
                        dateClose = DateTime.Now,
                        number = "",
                        warehouseName = ""
                    };

                    _context.Incomes.Add(tempIncome);
                }

                if (!await _context.Orders.AnyAsync(x => x.gNumber == jdo.gNumber))
                {
                    Order order = _mapper.Map<Order>(jdo);
                    _context.Orders.Add(order);
                }
                _context.SaveChanges();
            }
        }
    }
}