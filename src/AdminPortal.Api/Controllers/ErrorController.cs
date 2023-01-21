using AdminPortal.Api.Repositories;
using CommonService.Enums;
using CommonService.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace AdminPortal.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IErrorMessageTemplateRepository _messageRepo;

        public ErrorController(IConfiguration configuration, IErrorMessageTemplateRepository messageRepo)
        {
            _configuration = configuration;
            _messageRepo = messageRepo;
        }

        [Route("/api-error-handling")]
        public async Task<IActionResult> ErrorLocalDevelopment([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var env = _configuration.GetValue<string>("Environment");
            string stackTrace = null;
            if (env != "Production")
                stackTrace = context.Error.StackTrace;

            if(context.Error.GetType() == typeof(CustomizedException))
            {
                var code = ((CustomizedException)context.Error).Code;

                var error = await _messageRepo.GetErrorMessageAsync(code, Language);
                if(error == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new CommonApiResponse()
                    {
                        Success = false,
                        Message = context.Error.Message,
                        StackTrace = stackTrace
                    });
                }

                return StatusCode(StatusCodes.Status400BadRequest, new CommonApiResponse()
                {
                    Success = false,
                    ErrorCode = code,
                    Message = error.Message,
                    StackTrace = stackTrace
                });
                
            }
            else if (context.Error.GetType() == typeof(HttpResponseException))
            {
                return StatusCode((int)((HttpResponseException)context.Error.GetBaseException()).Status,
                    new CommonApiResponse
                    {
                        Success = false,
                        Message = ((HttpResponseException)context.Error.GetBaseException()).ReasonPhrase,
                        StackTrace = stackTrace
                    });
            }
            else
                return StatusCode(StatusCodes.Status400BadRequest, new CommonApiResponse()
                {
                    Success = false,
                    Message = context.Error.Message,
                    StackTrace = stackTrace
                });
        }
    }
}
