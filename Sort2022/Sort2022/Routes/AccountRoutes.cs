using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sort2022.Interfaces;
using Sort2022.Models;

namespace Sort2022.Routes
{
    public static class AccountRoutes
    {
        public static void RegisterRoutes(WebApplication app, WebApplicationBuilder builder)
        {
            app.MapPost("/login",
                [AllowAnonymous] async ([FromBody] User user,
                    [FromServices] ITokenService tokenService,
                    [FromServices] IUserRepositoryService userRepositoryService,
                    HttpResponse response) =>
                {
                    var verifiedUser = userRepositoryService.GetUser(user);
                    if ( verifiedUser != null )
                    {
                        var token = tokenService.BuildToken(
                            builder.Configuration["Jwt:Key"],
                            builder.Configuration["Jwt:Issuer"],
                            builder.Configuration["Jwt:Audience"],
                            verifiedUser
                            );

                        response.StatusCode = 200;
                        await response.WriteAsJsonAsync(new { token = token });
                        return;
                    }
                    else
                    {
                        response.StatusCode = 401;
                        return;
                    }
                }).WithName("Login").WithTags("Accounts");

            app.MapGet("/authorizedResource", [Authorize] () => "Action Succeeded")
                .WithName("Authorized").WithTags("Accounts").RequireAuthorization();
        }
    }
}
