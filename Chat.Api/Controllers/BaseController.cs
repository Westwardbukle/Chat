using System;
using System.Threading.Tasks;
using Chat.Common.Error;
using Chat.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Controllers
{
    public class BaseController : ControllerBase
    {
        protected async Task<ActionResult> ReturnResult<T, TM>(Task<T> task) where T : ResultContainer<TM>
        {
            var result = await task;

            if (result.ErrorType.HasValue)
            {
                return result.ErrorType switch
                {
                    ErrorType.NotFound => NotFound(),
                    ErrorType.BadRequest => BadRequest(),
                    ErrorType.Unauthorized => Unauthorized(),
                    ErrorType.Create => StatusCode(201),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            if (result.Data == null)
                return NoContent();

            return Ok(result.Data);
        }
    }
}