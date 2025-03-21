using Microsoft.IdentityModel.Tokens;
using POSV1.TenantAPI.Middleware;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

public class JwtService
{
    private readonly JwtSettingsProvider _jwtSettingsProvider;

    public JwtService(JwtSettingsProvider jwtSettingsProvider)
    {
        _jwtSettingsProvider = jwtSettingsProvider;
    }
    public void ConfigureToken()
    {

    }
    public JwtSecurityToken ValidateToken(HttpContext context, string token)
    {
        if (token != null)
        {
            var settings = _jwtSettingsProvider.GetSettings();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(settings.Secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,    
                ValidIssuer = settings.ValidIssuer,
                ValidAudience = settings.ValidAudience
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            //var userId = jwtToken.Claims.First(x => x.Type == "userId").Value;
            //context.Items["UserId"] = userId;
            return jwtToken;
        }
        return null;
    }
}
