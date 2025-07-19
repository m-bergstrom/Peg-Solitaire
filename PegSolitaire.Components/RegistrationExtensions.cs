using BlazorObservers.ObserverLibrary.DI;
using Finaltouch.DragDrop.Components;
using Microsoft.Extensions.DependencyInjection;
using PegSolitaire.Engine;

namespace PegSolitaire.Components;

public static class RegistrationExtensions
{
    public static IServiceCollection UsePegSolitaireComponents(this IServiceCollection services,
        PegSolitaireOptions? options = default)
    {
        if (options == null)
            options = new();

        services.AddResizeObserverService();
        services.AddScoped<DragDropInterop>();
        services.AddScoped<JsInterop>();

        services.UsePegSolitaireGameEngine(options);

        return services;
    }
}