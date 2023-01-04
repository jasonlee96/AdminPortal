using CommonService.Exceptions;
using CommonService.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CommonService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ErrorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("/api-error-handling")]
        public IActionResult ErrorLocalDevelopment([FromServices] IWebHostEnvironment webHostEnvironment)
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var env = _configuration.GetValue<string>("Environment");
            string stackTrace = null;
            if (env != "Production")
                stackTrace = context.Error.StackTrace;

            if (context.Error.GetType() == typeof(HttpResponseException))
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

        [Microsoft.AspNetCore.Mvc.Route("/error")]
        public IActionResult Error() => Problem();

    }
}
