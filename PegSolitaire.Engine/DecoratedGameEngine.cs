using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine;

/// <inheritdoc />
public abstract class DecoratedGameEngine : IGameEngine
{
    /// <inheritdoc />
    public abstract GameBoard GameBoard { get; protected internal set; }

    /// <inheritdoc />
    public abstract DateTimeOffset? StartTime { get; protected internal set; }

    /// <inheritdoc />
    public abstract DateTimeOffset? EndTime { get; protected internal set; }

    /// <inheritdoc />
    public abstract int MoveCount { get; protected internal set; }

    /// <inheritdoc />
    public abstract Stage Stage { get; protected internal set; }

    /// <inheritdoc />
    public abstract bool CanUndo { get; }

    /// <inheritdoc />
    public abstract bool CanRedo { get; }

    /// <inheritdoc />
    public void StartGame() => StartGame(this);

    /// <param name="gameEngine">The <see cref="IGameEngine"/> to use
    /// for public interface calls</param>
    /// <inheritdoc cref="IGameEngine.StartGame" />
    public abstract void StartGame(IGameEngine gameEngine);

    /// <inheritdoc />
    public List<Jump> GetValidMoves() => GetValidMoves(this);

    /// <param name="gameEngine">The <see cref="IGameEngine"/> to use
    ///     for public interface calls</param>
    /// <inheritdoc cref="IGameEngine.GetValidMoves" />
    public abstract List<Jump> GetValidMoves(IGameEngine gameEngine);

    /// <inheritdoc />
    public Stage MakeMove(Jump jump) => MakeMove(this, jump);

    /// <param name="gameEngine">The <see cref="IGameEngine"/> to use
    /// for public interface calls</param>
    /// <inheritdoc cref="IGameEngine.MakeMove" />
    public abstract Stage MakeMove(IGameEngine gameEngine, Jump jump);

    /// <inheritdoc />
    public void Undo() => Undo(this);

    /// <param name="gameEngine">The <see cref="IGameEngine"/> to use
    /// for public interface calls</param>
    /// <inheritdoc cref="IGameEngine.Undo" />
    public abstract void Undo(IGameEngine gameEngine);

    /// <inheritdoc />
    public void Redo() => Redo(this);

    /// <param name="gameEngine">The <see cref="IGameEngine"/> to use
    /// for public interface calls</param>
    /// <inheritdoc cref="IGameEngine.Redo" />
    public abstract void Redo(IGameEngine gameEngine);
}