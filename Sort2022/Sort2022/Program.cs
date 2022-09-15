using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sort2022.Data;
using Sort2022.Data.Contracts;
using Sort2022.Data.Repositories;
using Sort2022.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<sort2022Context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase"));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITaskRepository, TaskRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


app.MapGet("/", () => "Welcome to SORT 2022");

app.MapPost("/user", ([FromBody] User user, HttpResponse response) =>
{
    Console.WriteLine("/users was called!");
    response.StatusCode = 201;

    return user;
});

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
    [FromServices] ITaskRepository repository,
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
