using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Analysis;

/// <inheritdoc />
public abstract class DecoratedStateAnalyzer : IStateAnalyzer
{
    /// <inheritdoc />
    public Stage Analyze(GameBoard board)
    {
        return Analyze(this, board);
    }

    /// <param name="stateAnalyzer">The <see cref="IStateAnalyzer"/>
    /// to use for public interface calls</param>
    /// <inheritdoc cref="IStateAnalyzer.Analyze" />
    public abstract Stage Analyze(IStateAnalyzer stateAnalyzer, GameBoard board);
}