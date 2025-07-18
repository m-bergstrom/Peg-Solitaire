using BlazorObservers.ObserverLibrary.DI;
using Finaltouch.DragDrop.Components;
using Microsoft.Extensions.DependencyInjection;

namespace PegSolitaire.Components;

public static class RegistrationExtensions
{
    public static IServiceCollection AddPegSolitaireComponents(this IServiceCollection services)
    {
        services.AddResizeObserverService();
        services.AddScoped<DragDropInterop>();
        return services;
    }
}