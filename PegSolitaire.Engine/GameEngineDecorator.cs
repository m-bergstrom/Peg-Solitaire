using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine;

public class GameEngineDecorator : DecoratedGameEngine
{
    private IGameEngine _GameEngine;
    private readonly DecoratedGameEngine? _DecoratedGameEngine;

    public GameEngineDecorator(IGameEngine gameEngine)
    {
        _GameEngine = gameEngine;
        _DecoratedGameEngine = gameEngine as DecoratedGameEngine;
    }

    /// <inheritdoc />
    public override GameBoard GameBoard
    {
        get => (_DecoratedGameEngine ?? _GameEngine).GameBoard;
        protected internal set
        {
            if (_DecoratedGameEngine != null)
                _DecoratedGameEngine.GameBoard = value;
        }
    }

    /// <inheritdoc />
    public override DateTimeOffset? StartTime
    {
        get => (_DecoratedGameEngine ?? _GameEngine).StartTime;
        protected internal set
        {
            if (_DecoratedGameEngine != null)
                _DecoratedGameEngine.StartTime = value;
        }
    }

    /// <inheritdoc />
    public override DateTimeOffset? EndTime
    {
        get => (_DecoratedGameEngine ?? _GameEngine).EndTime;
        protected internal set
        {
            if (_DecoratedGameEngine != null)
                _DecoratedGameEngine.EndTime = value;
        }
    }

    /// <inheritdoc />
    public override int MoveCount
    {
        get => (_DecoratedGameEngine ?? _GameEngine).MoveCount;
        protected internal set
        {
            if (_DecoratedGameEngine != null)
                _DecoratedGameEngine.MoveCount = value;
        }
    }

    /// <inheritdoc />
    public override Stage Stage
    {
        get => (_DecoratedGameEngine ?? _GameEngine).Stage;
        protected internal set
        {
            if (_DecoratedGameEngine != null)
                _DecoratedGameEngine.Stage = value;
        }
    }

    /// <inheritdoc />
    public override bool CanUndo => (_DecoratedGameEngine ?? _GameEngine).CanUndo;

    /// <inheritdoc />
    public override bool CanRedo => (_DecoratedGameEngine ?? _GameEngine).CanRedo;

    /// <inheritdoc />
    public override void StartGame(IGameEngine gameEngine)
    {
        if (_DecoratedGameEngine != null)
            _DecoratedGameEngine.StartGame(gameEngine);
        else
            _GameEngine.StartGame();
    }

    /// <inheritdoc />
    public override List<Jump> GetValidMoves(IGameEngine gameEngine)
    {
        if (_DecoratedGameEngine != null)
            return _DecoratedGameEngine.GetValidMoves(gameEngine);
        return _GameEngine.GetValidMoves();
    }

    /// <inheritdoc />
    public override Stage MakeMove(IGameEngine gameEngine, Jump jump)
    {
        if (_DecoratedGameEngine != null)
            return _DecoratedGameEngine.MakeMove(gameEngine, jump);
        return _GameEngine.MakeMove(jump);
    }

    /// <inheritdoc />
    public override void Undo(IGameEngine gameEngine)
    {
        if (_DecoratedGameEngine != null)
            _DecoratedGameEngine.Undo(gameEngine);
        else
            _GameEngine.Undo();
    }

    /// <inheritdoc />
    public override void Redo(IGameEngine gameEngine)
    {
        if (_DecoratedGameEngine != null)
            _DecoratedGameEngine.Redo(gameEngine);
        else
            _GameEngine.Redo();
    }
}