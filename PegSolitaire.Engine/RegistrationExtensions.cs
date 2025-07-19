using Microsoft.Extensions.DependencyInjection;
using PegSolitaire.Engine.Analysis;
using PegSolitaire.Engine.GameState;
using PegSolitaire.Engine.Setup;
using PegSolitaire.Engine.Setup.Triangular;

namespace PegSolitaire.Engine;

public static class RegistrationExtensions
{
    public static IServiceCollection UsePegSolitaireGameEngine(this IServiceCollection services,
        PegSolitaireOptions? options = default)
    {
        if (options == null)
            options = new();

        if (options.UseAdvancedStateAnalyzer)
            services.AddScoped<IStateAnalyzer, DfsStateAnalyzer>();
        else
            services.AddScoped<IStateAnalyzer, SimpleStateAnalyzer>();

        services.AddKeyedScoped<INodeSetup, TriangularNodeSetup>(StringConstants.BoardShapes.Triangular);
        services.AddKeyedScoped<INodeAdjacencySetup, TriangularNodeAdjacencySetup>(StringConstants.BoardShapes
            .Triangular);

        services.AddScoped<IGameEngine, GameEngine>();

        services.AddScoped<Func<string, int, IGameEngine>>(f => (boardShape, boardSize) =>
        {
            var setup = f.GetKeyedService<INodeSetup>(boardShape);
            return ActivatorUtilities.CreateInstance<GameEngine>(f,
                new GameBoard(setup.GetInitalNodeState(boardSize).ToList()));
        });

        return services;
    }
}