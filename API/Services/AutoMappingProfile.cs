using AutoMapper;
using API.JDOs;
using API.Entitites;
using API.Entitites.WBEntities;
using API.DTOs;

public class AppMappingProfile : Profile
{
		public AppMappingProfile()
		{			
			CreateMap<JSONOrder, Order>();
            CreateMap<JSONOrder, OrderDetail>();

            CreateMap<JSONIncome, Income>();
            CreateMap<JSONIncome, IncomeDetail>();
		}
}