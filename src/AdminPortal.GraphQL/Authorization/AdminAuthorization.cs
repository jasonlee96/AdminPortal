using CommonService.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdminPortal.GraphQL.Authorization
{
    public class AdminAuthorizationRequirement : IAuthorizationRequirement
    {
    }
    public class AdminAuthorizationHandler : AuthorizationHandler<AdminAuthorizationRequirement>
    {
        private readonly ILogger<AdminAuthorizationHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminAuthorizationHandler(ILogger<AdminAuthorizationHandler> logger, IHttpContextAccessor httpContextAccessor)
        { 
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminAuthorizationRequirement requirement)
        {
            var currentContext = _httpContextAccessor.HttpContext;
            string key = "Authorization";
            var jwtToken = currentContext.Request.Headers.ContainsKey(key) ? currentContext.Request.Headers[key][0].Split(' ')[1] : null;

            if (jwtToken == null || !currentContext.User.Claims.Any())
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Unauthorized" });

            var userId = int.Parse(currentContext.User.Claims.Where(x => x.Type == "CurUserID").FirstOrDefault()?.Value);
            List<string> roles = currentContext.User.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault()?.Value.Split(",").ToList() ?? new List<string>();

            // validate Role
            if (!roles.Any(x => x == "SysAdmin")) return Task.CompletedTask;

            // validate Jwt Token from DB

            // Reset token last access time
            



            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
