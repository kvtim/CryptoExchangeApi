using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Models;
using UserManagement.Core.Dtos.User;

namespace UserManagement.Api.Mappings
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserDetailsDto>().ReverseMap();
            CreateMap<User, LoginUserDto>().ReverseMap();
            CreateMap<User, RegisterUserDto>().ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();
        }
    }
}
