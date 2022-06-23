using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.JDOs;

namespace API.Interfaces
{
    public interface IWBRepository
    {
        Task UploadIncomesToDB(List<JSONIncome> objData, IncomesDto incomesDto);
        Task UploadOrdersToDB(List<JSONOrder> objData, OrdersDto ordersDto);
        Task UploadSalesToDB(List<JSONSale> objData, SalesDto salesDto);
    }
}