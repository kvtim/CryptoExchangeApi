using AutoMapper;
using CurrencyManagement.Core.Models;
using CurrencyManagement.Core.Dtos.Currency;
using CurrencyManagement.Core.Dtos.CurrencyDimension;

namespace CurrencyManagement.Api.Mappings
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Currency, CurrencyDto>().ReverseMap();
            CreateMap<Currency, CurrencyWithDimensionDto>().ReverseMap();
            CreateMap<Currency, CreateCurrencyDto>().ReverseMap();
            CreateMap<Currency, UpdateCurrencyDto>().ReverseMap();

            CreateMap<CurrencyDimension, CurrencyDimensionDto>().ReverseMap();
        }
    }
}
