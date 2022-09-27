using Sort2022.Data.Contracts;
using Sort2022.Data.Repositories;
using Sort2022.Interfaces;
using Sort2022.Services;

namespace Sort2022
{
    public static class DependencyRegistration
    {

        public static void Register(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ITaskRepository, TaskRepository>();
            builder.Services.AddSingleton<IUserRepositoryService, UserRepositoryService>();
            builder.Services.AddSingleton<ITokenService, TokenService>();
        }
    }
}
