using CommonService.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CommonService.Jwt
{
    public class Jwt
    {
        public static string GenerateToken(string userId, Dictionary<string, string> claims = null)
        {
            if (claims == null)
                claims = new Dictionary<string, string>
                {
                    [ClaimTypes.NameIdentifier] = userId
                };
            else
                claims.Add(ClaimTypes.NameIdentifier, userId);

            var options = StartupExtension.Configuration.GetOptions<JwtOption>("Jwt");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(options.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.Select(x => new Claim(x.Key, x.Value))),
                Expires = DateTime.UtcNow.AddMinutes(options.KeyDuration ?? 30), // default 30 mins 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
