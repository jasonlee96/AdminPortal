using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Api.Controllers.AdminPortal
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/admin/[controller]")]
    [ApiController]
    public class BaseController : Controllers.BaseController
    {
    }
}
