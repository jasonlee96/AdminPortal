using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Api.Controllers.Public
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BaseController : Controllers.BaseController
    {
    }
}
