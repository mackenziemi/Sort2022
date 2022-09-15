using Microsoft.AspNetCore.Mvc;
using Sort2022.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Welcome to SORT 2022");

app.MapPost("/user", ([FromBody] User user, HttpResponse response) =>
{
    Console.WriteLine("/users was called!");
    
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine($"Username: {user.Username}, Password: {user.Password}");
    Console.ForegroundColor = ConsoleColor.White;

    response.StatusCode = 201;

    return user;    
})
    .Produces<User>();


app.Urls.Add("http://localhost:5757");

app.Run();
