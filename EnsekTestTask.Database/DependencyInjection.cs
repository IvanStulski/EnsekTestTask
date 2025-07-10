using EnsekTestTask.Database.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EnsekTestTask.Database;

public static class DependencyInjection
{
    public static void AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>((provider, options) =>
        {
            var dbSettingsOption = provider.GetRequiredService<IOptions<DbSettings>>().Value;
            options.UseNpgsql(dbSettingsOption.ConnectionString);
        });
    }

    public static async Task ApplyMigrations(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (context.Database != null)
        {
            await context.Database.MigrateAsync();
        }
    }
}
