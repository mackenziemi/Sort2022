using System.Reflection.Metadata.Ecma335;

namespace Sort2022.Routes
{
    public static class TestRoutes
    {
        public static void RegisterRoutes(WebApplication app)
        {
            app.MapGet("/", () => "Welcome to SORT 2022")
                .WithTags("Welcome").WithTags("Canary");

            app.MapGet("/test", () => "Test from a Routing Registration class")
                .WithTags("Test Message").WithTags("Canary");
        }
    }
}
