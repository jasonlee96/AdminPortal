using CommonService.Enums;
using CommonService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdminPortal.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        public string IPAddress
        {
            get
            {
                return HttpContext.Connection.RemoteIpAddress.ToString();
            }
        }

        public string JwtToken
        {
            get
            {
                string key = "Authorization";
                return HttpContext.Request.Headers.ContainsKey(key) ? HttpContext.Request.Headers[key][0].Split(' ')[1] : null;
            }
        }


        public int CurUserID
        {
            get
            {
                return int.Parse(HttpContext.User.Claims?.Where(x => x.Type == "UserID")?.FirstOrDefault()?.Value);
            }
        }

        public string UserName
        {
            get
            {
                return HttpContext.User.Claims?.Where(x => x.Type == "Username")?.FirstOrDefault()?.Value;
            }
        }
        public List<string> Roles
        {
            get
            {
                return HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault()?.Value.Split(",").ToList() ?? new List<string>();
            }
        }

        public LanguageCode Language
        {
            get
            {
                return HttpContext.User.Claims.Where(x => x.Type == "LANG").FirstOrDefault()?.Value.ToEnum<LanguageCode>() ?? LanguageCode.EN;
            }
        }

        public string Name
        {
            get
            {
                return HttpContext.User.Claims?.Where(x => x.Type == ClaimTypes.Name)?.FirstOrDefault()?.Value;
            }
        }

        public string Email
        {
            get
            {
                return HttpContext.User.Claims?.Where(x => x.Type == ClaimTypes.Email)?.FirstOrDefault()?.Value;
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<CommonApiResponse<R>> CommonApiResponse<R>(R result)
                    => Ok(new CommonApiResponse<R>(result));

        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult<CommonApiResponse> CommonApiResponse()
            => Ok(new CommonApiResponse());
    }
}
