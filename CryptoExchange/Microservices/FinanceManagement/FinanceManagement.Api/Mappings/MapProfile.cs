using AutoMapper;
using FinanceManagement.Core.Dtos.Wallet;
using FinanceManagement.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Api.Mappings
{
    public class MapProfile : Profile
    {
        public MapProfile() 
        {
            CreateMap<Wallet, WalletDto>().ReverseMap();
            CreateMap<Wallet, CreateWalletDto>().ReverseMap();
            CreateMap<Wallet, UpdateWalletDto>().ReverseMap();
        }
    }
}
