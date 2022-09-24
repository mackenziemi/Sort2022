using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sort2022.Data;
using Sort2022.Data.Contracts;
using Sort2022.Data.Repositories;
using Sort2022.Interfaces;
using Sort2022.Models;
using Sort2022.Services;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<sort2022Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, new string[]{ } }
    });
});

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddSingleton<IUserRepositoryService, UserRepositoryService>();
builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
        opt.Events = new JwtBearerEvents()
        {
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                // Ensure we always have an error and error description.
                if ( string.IsNullOrEmpty(context.Error) )
                    context.Error = "invalid_token";
                if ( string.IsNullOrEmpty(context.ErrorDescription) )
                    context.ErrorDescription = "This request requires a valid JWT access token to be provided";

                // Add some extra context for expired tokens.
                if ( context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException) )
                {
                    var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                    context.Response.Headers.Add("x-token-expired", authenticationException.Expires.ToString("o"));
                    context.ErrorDescription = $"The token expired on {authenticationException.Expires.ToString("o")}";
                }

                return context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    error = context.Error,
                    error_description = context.ErrorDescription
                }));
            }
        };
    });


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler("/error");

app.UseAuthentication();
app.UseAuthorization();


app.MapGet("/", () => "Welcome to SORT 2022");

app.MapPost("/login",
    [AllowAnonymous] async ([FromBody] User user,
        [FromServices] ITokenService tokenService,
        [FromServices] IUserRepositoryService userRepositoryService,
        HttpResponse response) =>
{
    var verifiedUser = userRepositoryService.GetUser(user);
    if(verifiedUser != null)
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

app.MapGet("/tasks", async ([FromServices] ITaskRepository repository) =>
{
    return await repository.GetAll();
});

app.MapGet("/tasks/{id}", async ([FromServices] ITaskRepository repository, int id) =>
{
    return await repository.GetById(id);
});

app.MapPost("/tasks", async (
    [FromBody] Sort2022.Data.Models.Task task, 
    ITaskRepository repository,
    HttpResponse response) =>
{
    var result = await repository.AddTask(task);
    if ( result != null )
    {
        response.StatusCode = 201;
        return task;
    }
    else
    {
        response.StatusCode = 500;
        return null;
    }
});

app.MapPut("/task/{id}/complete", async (
    [FromServices] ITaskRepository repository, 
    int id, 
    HttpResponse response) =>
{
    var result = await repository.CompleteTask(id);
    if(result)
    {
        response.StatusCode = 200;
        return result;
    }
    else
    {
        response.StatusCode = 500;
        return result;
    }

});

app.MapDelete("/tasks/{id}", async (
    [FromServices] ITaskRepository repository, 
    int id,
    HttpResponse response) =>
{
    var result = await repository.DeleteTask(id);
    if(result)
    {
        response.StatusCode = 200;
        return result;
    }
    else
    {
        response.StatusCode = 500;
        return result;
    }
});




app.Urls.Add("http://localhost:5757");

app.Run();
