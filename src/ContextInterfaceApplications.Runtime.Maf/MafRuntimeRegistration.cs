using ContextInterfaceApplications.Runtime;
using Microsoft.Extensions.DependencyInjection;

namespace ContextInterfaceApplications.Runtime.Maf;

public static class MafRuntimeRegistration
{
    public static IServiceCollection AddMafRuntimeSubstrate(this IServiceCollection services)
    {
        services.AddSingleton<IAgentRuntimeSubstrate, MafRuntimeSubstrate>();
        return services;
    }
}
