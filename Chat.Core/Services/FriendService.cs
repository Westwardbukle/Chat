using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto.Friend;
using Chat.Common.Exceptions;
using Chat.Common.RequestFeatures;
using Chat.Core.Abstract;
using Chat.Database.AbstractRepository;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Core.Services
{
    public class FriendService : IFriendService
    {
        private readonly ITokenService _tokenService;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly INotificationService _notification;

        public FriendService
        (   
            ITokenService tokenService, 
            IRepositoryManager repository,
            IMapper mapper,
            INotificationService notification
        )
        {
            _tokenService = tokenService;
            _repository = repository;
            _mapper = mapper;
            _notification = notification;
        }
        
        public async Task<FriendResponseDto> ConfirmFriendRequest(Guid unverifiedFriend, Guid userId)
        {
            if (_tokenService.GetCurrentUserId() != unverifiedFriend)
            {
                throw new IncorrectUserException();
            }
            
            var request = _repository.Friend.GetRequest(r => r.UserId == userId && r.FriendId == unverifiedFriend);

            if (request.FriendId != _tokenService.GetCurrentUserId())
            {
                throw new IncorrectUserException();
            }

            request.Confirmed = true;
            
            _repository.Friend.UpdateRequest(request);
            await _repository.SaveAsync();

            return _mapper.Map<FriendResponseDto>(request);
        }

        public async Task<FriendResponseDto> RejectFriendRequest(Guid userId , Guid requestId)
        {
            var request = _repository.Friend.GetRequest(r => r.UserId == userId && r.FriendId == requestId);
            
            if (request.FriendId != _tokenService.GetCurrentUserId())
            {
                throw new IncorrectUserException();
            }
            
            _repository.Friend.DeleteRequest(request);
            await _repository.SaveAsync();

            return _mapper.Map<FriendResponseDto>(request);
        }


        public async Task<List<FriendResponseDto>> GetAllFriendsOfUser(Guid userId , FriendParameters friendParameters)
        {
            if (_tokenService.GetCurrentUserId() != userId)
            {
                throw new IncorrectUserException();
            }
            
            var allFriends = await _repository.Friend.GetAllFriends(userId, false, friendParameters);

            var usersFriends = _mapper.Map<List<FriendResponseDto>>(allFriends);

            return usersFriends;
        }
        
        /*public async Task<List<FriendResponseDto>> GetAllFriendsRequest(FriendParameters friendParameters)
        {
            var user = _tokenService.GetCurrentUserId();

            var allRequests = await _repository.Friend.GelAllRequest(user, false, friendParameters);

            var request = _mapper.Map<List<FriendResponseDto>>(allRequests);

            return request;
        }*/
        
    }
}