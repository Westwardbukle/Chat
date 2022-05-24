using System;
using System.Threading.Tasks;
using Chat.Common.Dto;
using Chat.Common.Dto.Code;
using Chat.Common.Dto.Login;
using Chat.Common.Result;
using Chat.Common.User;
using Chat.Core.Auth;
using Chat.Core.Restoring;
using Chat.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IRestoringCode _restoringCode;

        public AuthController
        (
            IAuthService authService,
            IRestoringCode restoringCode
        )
        {
            _authService = authService;
            _restoringCode = restoringCode;
        }

        /// <summary>
        /// User registrarion
        /// </summary>
        /// <param name="registerUserDto"></param>
        /// <response code="201">User added</response>
        /// <response code="422">Validation Exception</response>
        /// <response code="400">User already registered </response>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Registration([FromBody] RegisterUserDto registerUserDto)
            => await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_authService.Registration(registerUserDto));


        /// <summary>
        /// User login
        /// </summary>
        /// <param name="loginUserDto"></param>
        /// <returns>Bearer token </returns>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> Login(LoginUserDto loginUserDto)
            => await ReturnResult<ResultContainer<UserResponseDto>, UserResponseDto>
                (_authService.Login(loginUserDto));

        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeDto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> EmailConfirm(CodeDto codeDto)
            => await ReturnResult<ResultContainer<CodeResponseDto>, CodeResponseDto>
                (_authService.CodeСonfirmation(codeDto));


        /// <summary>
        /// sending code
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<ActionResult> SendRestoringCode(UserDto userDto)
            => await ReturnResult<ResultContainer<CodeResponseDto>, CodeResponseDto>
                (_restoringCode.SendRestoringCode(userDto));
    }
}