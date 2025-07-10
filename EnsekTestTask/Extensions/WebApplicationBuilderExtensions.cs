using EnsekTestTask.Database.Models.Configuration;

namespace EnsekTestTask.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings"));
    }
}
