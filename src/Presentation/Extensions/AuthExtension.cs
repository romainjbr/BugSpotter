using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Presentation.Extensions;

public static class AuthExtension
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration config)
    {
        var jwtOptions = config.GetSection("Jwt").Get<JwtOptions>(); 
        
        var keyText = jwtOptions?.Key;
        var keyBytes = Encoding.UTF8.GetBytes(keyText!);
        var signingKey = new SymmetricSecurityKey(keyBytes);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {   
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false, 
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ClockSkew = TimeSpan.Zero
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
        });

        return services;
    }
}
