using EnsekTestTask.Core.Services.Abstractions;
using EnsekTestTask.Core.Services.Realizations;
using Microsoft.Extensions.DependencyInjection;

namespace EnsekTestTask.Core;

public static class DependencyInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IMeterService, MeterService>();
    }
}
