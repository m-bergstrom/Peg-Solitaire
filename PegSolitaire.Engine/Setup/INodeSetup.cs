using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Setup;

/// <summary>
/// Sets up initial <see cref="GameNode"/> graph for a
/// <see cref="GameBoard"/>
/// </summary>
public interface INodeSetup
{
    /// <summary>
    /// Sets up the initial graph of <see cref="GameNode"/>s for a
    /// <see cref="GameBoard"/>
    /// </summary>
    /// <param name="size">The size of the game board, usually in rows</param>
    /// <returns>A graph of nodes for the board's initial state</returns>
    List<GameNode> GetInitalNodeState(int size);
}