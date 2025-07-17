using BlazorObservers.ObserverLibrary.DI;
using Microsoft.Extensions.DependencyInjection;

namespace PegSolitaire.Components;

public static class RegistrationExtensions
{
    public static IServiceCollection AddPegSolitaireComponents(this IServiceCollection services)
    {
        services.AddResizeObserverService();
        return services;
    }
}