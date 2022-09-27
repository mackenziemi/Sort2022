using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sort2022.Data.Contracts;

namespace Sort2022.Routes
{
    public static class TaskRoutes
    {
        public static void RegisterRoutes(WebApplication app)
        {
            app.MapGet("/tasks", [Authorize] async ([FromServices] ITaskRepository repository) =>
            {
                return await repository.GetAll();
            }).WithName("Get all tasks").WithTags("Tasks").RequireAuthorization();

            app.MapGet("/tasks/{id}", [Authorize] async ([FromServices] ITaskRepository repository, int id) =>
            {
                return await repository.GetById(id);
            }).WithName("Get task by Id").WithTags("Tasks").RequireAuthorization();

            app.MapPost("/tasks", [Authorize] async (
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
                }).WithName("Add new task").WithTags("Tasks").RequireAuthorization();

            app.MapPut("/task/{id}/complete", [Authorize] async (
                [FromServices] ITaskRepository repository,
                int id,
                HttpResponse response) =>
            {
                var result = await repository.CompleteTask(id);
                if ( result )
                {
                    response.StatusCode = 200;
                    return result;
                }
                else
                {
                    response.StatusCode = 500;
                    return result;
                }
            }).WithName("Update task").WithTags("Tasks").RequireAuthorization();

            app.MapDelete("/tasks/{id}", [Authorize] async (
                [FromServices] ITaskRepository repository,
                int id,
                HttpResponse response) =>
            {
                var result = await repository.DeleteTask(id);
                if ( result )
                {
                    response.StatusCode = 200;
                    return result;
                }
                else
                {
                    response.StatusCode = 500;
                    return result;
                }
            }).WithName("Remove tasks").WithTags("Tasks").RequireAuthorization();
        }
    }
}
