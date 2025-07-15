using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Analysis;

/// <summary>
/// Analyzes a Peg Solitaire game's state for win or loss conditions
/// </summary>
public interface IStateAnalyzer
{
    /// <summary>
    /// Analyzes a Peg Solitaire game's state for win or loss conditions
    /// </summary>
    /// <param name="board">The <see cref="GameBoard"/> to analyze</param>
    /// <returns>The state of the game the analyzer determines from
    /// the game's state</returns>
    Stage Analyze(GameBoard board);
}