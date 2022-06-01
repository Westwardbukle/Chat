using System.Collections.Generic;
using AutoMapper;
using Chat.Common.Dto;
using Chat.Common.Dto.Chat;
using Chat.Common.Dto.Message;
using Chat.Common.Dto.User;
using Chat.Database.Model;

namespace Chat.Core.ProFiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<UserModel, UserResponseDto>();
            
            CreateMap<UserModel, UserModelDto>();
            
            
            // Return list chats, in ChatService, GetAllChats method
            CreateMap<ChatModel, ChatResponseDto>();
            CreateMap<ChatModel, List<ChatResponseDto>>();
            
            // Return List messages, in MessageService, GetAllMessageInChat method
            CreateMap<MessageModel, MessagesResponseDto>();
            CreateMap<MessageModel, List<MessagesResponseDto>>();

            // Return list users, in AuthService, GetAllUsers method 
            CreateMap<UserModel, GetAllUsersDto>();
        }
    }
}