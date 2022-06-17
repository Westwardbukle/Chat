using System;
using System.Threading.Tasks;
using Chat.Common.Code;
using Chat.Common.Dto.Code;
using Chat.Common.Exceptions;
using Chat.Core.Abstract;
using Chat.Database.AbstractRepository;
using Chat.Database.Model;

namespace Chat.Core.Services
{
    public class RestoringCodeServiceService : IRestoringCodeService
    {
        private readonly ICodeService _code;
        private readonly ISmtpService _smtpService;
        private readonly ITokenService _tokenService;
        private readonly IRepositoryManager _repository;

        public RestoringCodeServiceService
        (
            ICodeService code,
            ISmtpService smtpService,
            ITokenService tokenService,
            IRepositoryManager repository
        )
        {
            _code = code;
            _smtpService = smtpService;
            _tokenService = tokenService;
            _repository = repository;
        }


        public async Task SendRestoringCodeAsync(Guid userId)
        {
            var user = await _repository.User.GetUserAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            var lastCode = await _repository.Code.GetCodeAsync(c => c.Id == user.Id);

            if (lastCode is null)
            {
                var newMailCode = _code.GenerateRestoringCode();

                await _smtpService.SendEmailAsync(user.Email, newMailCode);

                var newCode = new CodeModel()
                {
                    Id = user.Id,
                    Code = newMailCode,
                    CodePurpose = CodePurpose.ConfirmEmail,
                    DateCreated = DateTime.Now,
                    DateExpiration = DateTime.Now.AddHours(2),
                    UserId = user.Id
                };

                await _repository.Code.CreateCodeAsync(newCode);
            }

            _repository.Code.DeleteCode(lastCode);

            var mailCode = _code.GenerateRestoringCode();

            await _smtpService.SendEmailAsync(user.Email, mailCode);

            var code = new CodeModel()
            {
                Id = user.Id,
                Code = mailCode,
                CodePurpose = CodePurpose.ConfirmEmail,
                DateCreated = DateTime.Now,
                DateExpiration = DateTime.Now.AddHours(2),
                UserId = user.Id
            };

            await _repository.Code.CreateCodeAsync(code);
        }

        public async Task ConfirmEmailCodeAsync(CodeDto codeDto)
        {
            var user = await _repository.User.GetUserAsync(u => u.Id == _tokenService.GetCurrentUserId());

            if (user is null)
            {
                throw new UserNotFoundException();
            }

            var code = await _repository.Code.GetCodeAsync(c => c.UserId == user.Id);

            if (code is null) throw new ActivationСodeТotFoundException();

            if (code.CodePurpose != CodePurpose.ConfirmEmail && code.DateExpiration > DateTime.Now)
            {
                throw new EmailCodeNotValidException();
            }

            if (codeDto.Code != code.Code)
            {
                throw new EmailCodeNotValidException();
            }

            await ActivateEmailUserAsync(user);

            _repository.Code.DeleteCode(code);
            await _repository.SaveAsync();
        }

        public async Task ActivateEmailUserAsync(UserModel user)
        {
            user.Active = true;
            user.DateTimeActivation = DateTime.Now;

            _repository.User.UpdateUser(user);
            await _repository.SaveAsync();
        }
    }
}