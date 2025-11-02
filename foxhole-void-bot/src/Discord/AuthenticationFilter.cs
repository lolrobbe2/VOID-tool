using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
namespace FoxholeBot.Discord
{


public class DiscordAuthorizeFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
            .OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>().Any() == true
            || context.MethodInfo.GetCustomAttributes(true)
            .OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>().Any();

        if (hasAuthorize)
        {
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2" // or "Bearer" if you're using JWT
                            }
                        },
                        new[] { "identify" }
                    }
                }
            };
        }
    }
}

}
