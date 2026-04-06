using Microsoft.AspNetCore.Builder;
using ContextInterfaceApplications.Web.Services;
using ContextInterfaceApplications.Web.Workflows.Demo;
using Microsoft.Extensions.DependencyInjection;

namespace ContextInterfaceApplications.Tests;

internal static class TestServiceFactory
{
    public static IServiceProvider CreateServices()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddRazorComponents();
        builder.Services.AddSingleton<IAuthoredAffordanceResolver, DemoAuthoredAffordanceResolver>();
        builder.Services.AddSingleton<IActionComponentResolver, ActionComponentResolver>();
        builder.Services.AddSingleton<IStepComponentResolver, DemoStepComponentResolver>();
        builder.Services.AddSingleton<IStepSurfaceMetadataResolver, DemoStepSurfaceMetadataResolver>();
        builder.Services.AddSingleton<IAgentInterfaceSnapshotBuilder, AgentInterfaceSnapshotBuilder>();
        var app = builder.Build();
        return app.Services;
    }
}
