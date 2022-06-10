using System;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Common.Dto.Friend;
using Chat.Common.Exceptions;
using Chat.Core.Abstract;
using Chat.Database.AbstractRepository;
using Chat.Database.Model;

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
        
        public async Task<FriendResponseDto> ConfirmFriendRequest(Guid id)
        {
            var request = _repository.Friend.GetRequest(r => r.Id == id);

            if (request.FriendId != _tokenService.GetCurrentUserId())
            {
                throw new IncorrectUserException();
            }

            request.Confirmed = true;
            
            _repository.Friend.UpdateRequest(request);
            await _repository.SaveAsync();

            return _mapper.Map<FriendResponseDto>(request);
            
            
        }
  

    }
}