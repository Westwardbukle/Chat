using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto;
using Chat.Common.Dto.Login;
using Chat.Common.Dto.User;
using Chat.Common.Result;
using Chat.Database.Model;

namespace Chat.Common.ProFiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<UserModel, UserResponseDto>();
            CreateMap<UserModel, ResultContainer<UserResponseDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(f => f));
            CreateMap<UserModel, UserModelDto>();
            
        }
    }
}