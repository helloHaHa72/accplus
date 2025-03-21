
using BaseAppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using POSV1.TenantAPI.Startup;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSV1.TenantAPI.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtService _jwtService;
    public JwtMiddleware(RequestDelegate next, JwtService jwtService)
    {
        _next = next;
        _jwtService = jwtService;
    }

    public async Task InvokeAsync(HttpContext context)
    {

        if (context.Request.Path.StartsWithSegments("/api/auth/login", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        // Retrieve the JWT from the Authorization header
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            // Extract token from header
            var token = authorizationHeader.Substring("Bearer ".Length).Trim();

            try
            {
                var user = context.User;
                if (user?.Identity?.IsAuthenticated == true)
                {
                    Console.WriteLine($"User is authenticated: {user.Identity.Name}");
                }

                var jwtToken = _jwtService.ValidateToken(context, token);

                // Assuming you have claims "origin" and "tenant-id" in the token
                var origin = jwtToken?.Claims.FirstOrDefault(c => c.Type == "origin")?.Value;
                var tenantId = jwtToken?.Claims.FirstOrDefault(c => c.Type == "X-Tenant-Id")?.Value;

                // Check if the token has the "superadmin" role
                var hasSuperAdminRole = jwtToken?.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == nameof(EnumApplicationUserType.SuperAdmin));

                if (hasSuperAdminRole == true)
                {
                    // Add context parameters to the HttpContext
                    context.Items["Origin"] = origin;
                    context.Items["TenantId"] = tenantId;
                }
                else
                {
                    // Check for X-Tenant-Id header
                    var tenantHeader = context.Request.Headers["X-Tenant-Id"].FirstOrDefault();

                    if (!string.IsNullOrEmpty(tenantHeader))
                    {
                        if (tenantHeader != tenantId)
                        {
                            throw new Exception("Tenant-Id Mismatch in headers");
                        }
                        //validate againts master db -> tenant record and try creating db connection
                        //read whole db object and store it in context

                        // Validate and store the X-Tenant-Id header value in context
                        context.Items["Origin"] = origin;
                        context.Items["TenantId"] = tenantHeader;
                    }
                    else
                    {
                        throw new Exception("Missing Tenant-Id in headers");
                    }
                }
            }
            catch (Exception)
            {
                // Handle invalid JWT or error in decoding (optional logging)
                context.Items["Origin"] = null;
                context.Items["TenantId"] = null;
                throw;
            }
        }

        // Call the next middleware in the pipeline
        await _next(context);
    }
}

