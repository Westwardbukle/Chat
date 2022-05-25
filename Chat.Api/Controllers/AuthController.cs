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
        private readonly IRestoringCodeService _restoringCodeService;

        public AuthController
        (
            IAuthService authService,
            IRestoringCodeService restoringCodeService
        )
        {
            _authService = authService;
            _restoringCodeService = restoringCodeService;
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
    }
}