using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Analysis;

/// <inheritdoc />
/// <summary>Analyzes a Peg Solitaire game's state for win or loss
/// conditions using a depth-first search to identify situations in
/// which no win is possible</summary>
public class DfsStateAnalyzer : DecoratedStateAnalyzer
{
    /// <inheritdoc />
    public override Stage Analyze(IStateAnalyzer stateAnalyzer, GameBoard board)
    {
        if (board.Jumps.Any(j => j.IsValid))
            return CanWin(board) ? Stage.InProgress : Stage.Lost;
        if (board.Nodes.Count(n => n.Occupied) == 1)
            return Stage.Won;
        return Stage.Lost;
    }

    private bool CanWin(GameBoard board)
    {
        if (board.Nodes.Count(n => n.Occupied) == 1)
            return true;
        if (!board.Jumps.Any(j => j.IsValid))
            return false;

        foreach (var jump in board.Jumps.Where(j => j.IsValid))
            if (CanWin(jump.Execute(board)))
                return true;

        return false;
    }
}