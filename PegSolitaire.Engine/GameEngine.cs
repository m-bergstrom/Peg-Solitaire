using PegSolitaire.Engine.Analysis;
using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine;

/// <inheritdoc />
public sealed class GameEngine : DecoratedGameEngine
{
    private readonly IStateAnalyzer _StateAnalyzer;
    private readonly Stack<(GameBoard board, Jump move)> _UndoStack = new();
    private readonly Stack<(GameBoard board, Jump move)> _RedoStack = new();

    /// <inheritdoc />
    public override GameBoard GameBoard { get; protected internal set; }

    /// <inheritdoc />
    public override DateTimeOffset? StartTime { get; protected internal set; }

    /// <inheritdoc />
    public override DateTimeOffset? EndTime { get; protected internal set; }

    /// <inheritdoc />
    public override int MoveCount { get; protected internal set; }

    /// <inheritdoc />
    public override Stage Stage { get; protected internal set; }

    /// <inheritdoc />
    public override bool CanUndo => Stage == Stage.InProgress && _UndoStack.Count > 0;

    /// <inheritdoc />
    public override bool CanRedo => Stage == Stage.InProgress && _RedoStack.Count > 0;

    public GameEngine(GameBoard gameBoard, IStateAnalyzer stateAnalyzer)
    {
        GameBoard = gameBoard;
        _StateAnalyzer = stateAnalyzer;
    }

    /// <inheritdoc />
    public override void StartGame(IGameEngine gameEngine)
    {
        CheckStage(Stage.NotYetStarted);
        Stage = Stage.InProgress;
        StartTime = DateTimeOffset.Now;
    }

    /// <inheritdoc />
    public override List<Jump> GetValidMoves(IGameEngine gameEngine)
    {
        CheckStage(Stage.InProgress);
        return gameEngine.GameBoard.Jumps.Where(j => j.IsValid).ToList();
    }

    /// <inheritdoc />
    public override Stage MakeMove(IGameEngine gameEngine, Jump jump)
    {
        CheckStage(Stage.InProgress);
        if (!GameBoard.Jumps.Contains(jump))
            throw new InvalidOperationException("This move is from a different board");
        jump.CheckValidity();
        _UndoStack.Push((GameBoard, jump));
        _RedoStack.Clear();

        GameBoard = jump.Execute(GameBoard);
        ++MoveCount;
        Stage = _StateAnalyzer.Analyze(GameBoard);

        if (Stage != Stage.InProgress)
            EndTime = DateTimeOffset.Now;

        return Stage;
    }

    /// <inheritdoc />
    public override void Undo(IGameEngine gameEngine)
    {
        CheckStage(Stage.InProgress);
        if (_UndoStack.Count == 0)
            throw new InvalidOperationException("No moves to undo");
        var (board, move) = _UndoStack.Pop();
        _RedoStack.Push((GameBoard, move));
        GameBoard = board;
    }

    /// <inheritdoc />
    public override void Redo(IGameEngine gameEngine)
    {
        CheckStage(Stage.InProgress);
        if (_RedoStack.Count == 0)
            throw new InvalidOperationException("No moves to redo");
        GameBoard = _RedoStack.Pop().board;
    }

    private void CheckStage(Stage requiredStage)
    {
        if (Stage != requiredStage)
            throw new InvalidOperationException($"Requires stage {requiredStage}");
    }
}