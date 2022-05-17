using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto;
using Chat.Common.Result;

namespace Chat.Common.ProFiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<UserResponseDto, ResultContainer<UserResponseDto>>()
                .ForMember("Data", opt =>
                    opt.MapFrom(f => f));
        }
    }
}