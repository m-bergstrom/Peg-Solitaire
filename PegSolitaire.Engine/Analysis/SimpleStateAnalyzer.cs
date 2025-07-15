using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Analysis;

/// <inheritdoc />
public class SimpleStateAnalyzer : DecoratedStateAnalyzer
{
    /// <inheritdoc />
    public override Stage Analyze(IStateAnalyzer stateAnalyzer, GameBoard board)
    {
        if (board.Jumps.Any(j => j.IsValid))
            return Stage.InProgress;
        if (board.Nodes.Count(n => n.Occupied) == 1)
            return Stage.Won;
        return Stage.Lost;
    }
}