using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto;
using Chat.Common.Result;
using Chat.Database.Model;

namespace Chat.Common.ProFiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<UserModel, RegisterResponseDto>();
            CreateMap<UserModel, ResultContainer<RegisterResponseDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(f => f));
        }
    }
}