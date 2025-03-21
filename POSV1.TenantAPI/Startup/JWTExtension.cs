using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using POSV1.TenantAPI.Middleware;
using System.Text;

namespace POSV1.TenantAPI.Startup
{

    public static class JWTExtension
    {
        public static void ConfigureJWT(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            // Bind JwtSettings from configuration
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));

            // Register the thread-safe settings provider
            builder.Services.AddSingleton<JwtSettingsProvider>();
            builder.Services.AddSingleton<JwtService>();

            var jwtSettings = builder.Configuration.GetSection("JWT").Get<JwtSettings>();

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            Console.WriteLine($"Authentication challenge: {context.Error}, {context.ErrorDescription}");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("Token validated successfully");
                            return Task.CompletedTask;
                        }
                    };
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.ValidIssuer,
                        ValidAudience = jwtSettings.ValidAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                    };
                });
            builder.Services.AddAuthorization();

        }
    }
}
