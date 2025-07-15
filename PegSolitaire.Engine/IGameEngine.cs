using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine;

/// <summary>
/// Represents a Peg Solitaire game
/// </summary>
public interface IGameEngine
{
    /// <summary>
    /// Gets the game board for this Peg Solitaire game
    /// </summary>
    GameBoard GameBoard { get; }

    /// <summary>
    /// Gets the time the game started
    /// </summary>
    DateTimeOffset? StartTime { get; }

    /// <summary>
    /// Gets the time the game ended
    /// </summary>
    DateTimeOffset? EndTime { get; }

    /// <summary>
    /// Gets the number of moves in this Peg Solitaire game
    /// </summary>
    int MoveCount { get; }

    /// <summary>
    /// Gets the current state of this Peg Solitaire game
    /// </summary>
    Stage Stage { get; }

    /// <summary>
    /// Gets whether the user can undo the last move
    /// </summary>
    bool CanUndo { get; }

    /// <summary>
    /// Gets whether the user can redo a move
    /// </summary>
    bool CanRedo { get; }

    /// <summary>
    /// Starts the game
    /// </summary>
    void StartGame();

    /// <summary>
    /// Retrieves a list of valid moves, if any
    /// </summary>
    /// <returns>A list of valid moves</returns>
    List<Jump> GetValidMoves();

    /// <summary>
    /// Make a move in this Peg Solitaire game
    /// </summary>
    /// <param name="jump">The move we want to make</param>
    /// <returns>The state of the Peg Solitaire game after this move</returns>
    Stage MakeMove(Jump jump);

    /// <summary>
    /// Undoes the last move
    /// </summary>
    void Undo();

    /// <summary>
    /// Redoes a move
    /// </summary>
    void Redo();
}