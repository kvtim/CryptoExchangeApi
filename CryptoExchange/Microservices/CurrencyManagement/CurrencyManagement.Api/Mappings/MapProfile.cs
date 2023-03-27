using AutoMapper;
using CurrencyManagement.Core.Models;
using CurrencyManagement.Data.Dtos.Currency;

namespace CurrencyManagement.Api.Mappings
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Currency, CurrencyDto>().ReverseMap();
            CreateMap<Currency, CreateCurrencyDto>().ReverseMap();
            CreateMap<Currency, UpdateCurrencyDto>().ReverseMap();
        }
    }
}
