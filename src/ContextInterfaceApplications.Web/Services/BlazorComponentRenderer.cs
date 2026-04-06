using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ContextInterfaceApplications.Web.Services;

public interface IBlazorComponentRenderer
{
    Task<string> RenderAsync<TComponent>(IReadOnlyDictionary<string, object?> parameters)
        where TComponent : IComponent;
}

public sealed class BlazorComponentRenderer : IBlazorComponentRenderer
{
    private readonly IServiceScopeFactory _scopeFactory;

    public BlazorComponentRenderer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<string> RenderAsync<TComponent>(IReadOnlyDictionary<string, object?> parameters)
        where TComponent : IComponent
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
        await using var renderer = new HtmlRenderer(services, loggerFactory);

        return await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var parameterView = ParameterView.FromDictionary(parameters.ToDictionary(pair => pair.Key, pair => pair.Value));
            var rootComponent = await renderer.RenderComponentAsync<TComponent>(parameterView);
            return rootComponent.ToHtmlString();
        });
    }
}
