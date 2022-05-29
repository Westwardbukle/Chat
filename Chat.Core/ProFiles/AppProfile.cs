using System.Collections.Generic;
using AutoMapper;
using Chat.Common.Dto;
using Chat.Common.Dto.Chat;
using Chat.Common.Dto.User;
using Chat.Common.Result;
using Chat.Common.User;
using Chat.Database.Model;

namespace Chat.Core.ProFiles
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
            
            
            
            
            CreateMap<ChatModel, ChatResponseDto>();
            
            CreateMap<ChatModel, List<ChatResponseDto>>();
            
            
            



            CreateMap<UserModel, GetAllUsersDto>();
            
            CreateMap<UserModel, List<GetAllUsersDto>>();
        }
    }
}