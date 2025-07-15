using PegSolitaire.Engine.Analysis;
using PegSolitaire.Engine.GameState;
using PegSolitaire.Engine.Setup;
using PegSolitaire.Engine.Setup.Triangular;

namespace PegSolitaire.Engine.Tests;

[TestClass]
public class GameEngineTests
{
    [TestMethod]
    public void GameEngine_PreGameStateIsCorrect()
    {
        var engine = CreateGameEngine();
        Assert.IsNull(engine.StartTime, "The start time shouldn't be recorded before starting the game");
        Assert.IsNull(engine.EndTime, "The end time shouldn't be recorded before starting the game");
        Assert.AreEqual(Stage.NotYetStarted, engine.Stage,
            $"The game state should be {Stage.NotYetStarted} before starting the game");
        Assert.AreEqual(false, engine.CanUndo, "Should not be able to undo before starting the game");
        Assert.AreEqual(false, engine.CanRedo, "Should not be able to redo before starting the game");
        Assert.AreEqual(0, engine.MoveCount, "There shouldn't be any moves recorded before starting the game");
    }

    [TestMethod]
    public void GameEngine_StartsGame()
    {
        var engine = CreateGameEngine();
        engine.StartGame();
        Assert.IsNotNull(engine.StartTime, "Starting the game should record the start time");
        Assert.AreEqual(Stage.InProgress, engine.Stage);
        Assert.IsFalse(engine.CanUndo,
            "Shouldn't be able to undo before making any moves, but the game engine thought so");
        Assert.IsFalse(engine.CanRedo,
            "Shouldn't be able to redo before making any moves, but the game engine thought so");
        Assert.IsNull(engine.EndTime, "The end time shouldn't be recorded before ending the game");
    }

    [TestMethod]
    public void GameEngine_GetsValidMoves_Correct()
    {
        var engine = CreateGameEngine();
        engine.StartGame();
        var validMoveCount = engine.GetValidMoves().Count;
        Assert.AreEqual(2, validMoveCount,
            $"There should be 2 valid moves at the start of the game, but the game engine thought there were {validMoveCount}");
        foreach (var validMove in engine.GetValidMoves())
            Assert.IsTrue(validMove.IsValid);


        engine.GameBoard.Nodes.SetState([4]);
        validMoveCount = engine.GetValidMoves().Count;
        Assert.AreEqual(0, validMoveCount,
            $"There should be 0 valid moves after the game is won, but the game engine thought there were {validMoveCount}");
        foreach (var validMove in engine.GetValidMoves())
            Assert.IsTrue(validMove.IsValid);

        engine.GameBoard.Nodes.SetState([0, 7, 8, 10, 11, 13, 14]);
        validMoveCount = engine.GetValidMoves().Count;
        Assert.AreEqual(6, validMoveCount,
            $"There should be 6 valid moves at this point in the game, but the game engine thought there were {validMoveCount}");
        foreach (var validMove in engine.GetValidMoves())
            Assert.IsTrue(validMove.IsValid);
    }

    [TestMethod]
    public void GameEngine_GetValidMoves_BeforeStartFails()
    {
        var engine = CreateGameEngine();
        Assert.ThrowsException<InvalidOperationException>(() => engine.GetValidMoves());
    }

    [TestMethod]
    public void GameEngine_MakesMove()
    {
        var engine = CreateGameEngine();
        engine.StartGame();
        var move = engine.GetValidMoves().First();
        engine.MakeMove(move);
        Assert.AreEqual(1, engine.MoveCount,
            $"After a move, move count should be 1, but the game engine thought it was {engine.MoveCount}");
        Assert.IsTrue(engine.CanUndo, "After a move, should be able to undo, but the game engine didn't think so");
        Assert.IsFalse(engine.CanRedo, "After a move, shouldn't be able to redo, but the game engine thought so");

        engine.GameBoard.Nodes.SetState([0, 1]);
        move = engine.GetValidMoves().Single();
        engine.MakeMove(move);
        engine.GameBoard.Nodes.CheckState([3], "After the move, ");
        Assert.AreEqual(Stage.Won, engine.Stage,
            $"After the last move, stage should be won, but the game engine thought is was {engine.Stage}");
    }

    [TestMethod]
    public void GameEngine_MakesMove_BeforeStartFails()
    {
        var engine = CreateGameEngine();
        var move = engine.GameBoard.IndexedJumps[engine.GameBoard.Nodes[3]].First(j => j.IsValid);
        Assert.ThrowsException<InvalidOperationException>(() => engine.MakeMove(move));
    }

    [TestMethod]
    public void GameEngine_MakesMove_InvalidMoveFails()
    {
        var engine = CreateGameEngine();
        engine.StartGame();
        var move = engine.GameBoard.IndexedJumps[engine.GameBoard.Nodes[3]].First(j => !j.IsValid);
        Assert.ThrowsException<InvalidOperationException>(() => engine.MakeMove(move));
    }

    [TestMethod]
    public void GameEngine_UndoesMove()
    {
        var engine = CreateGameEngine();
        engine.StartGame();
        var move = engine.GetValidMoves().First();
        engine.MakeMove(move);
        engine.GameBoard.Nodes.CheckState([0, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14],
            "After the first move, ");
        engine.Undo();
        engine.GameBoard.Nodes.CheckState([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14],
            "After undoing the first move, ");
    }

    [TestMethod]
    public void GameEngine_UndoesMove_FailsIfNotInProgress()
    {
        var engine = CreateGameEngine();
        Assert.ThrowsException<InvalidOperationException>(() => engine.Undo());
    }

    [TestMethod]
    public void GameEngine_RedoesMove()
    {
        var engine = CreateGameEngine();
        engine.StartGame();
        var move = engine.GetValidMoves().First();
        engine.MakeMove(move);
        engine.GameBoard.Nodes.CheckState([0, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14],
            "After the first move, ");
        engine.Undo();
        engine.GameBoard.Nodes.CheckState([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14],
            "After undoing the first move, ");
        engine.Redo();
        engine.GameBoard.Nodes.CheckState([0, 2, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14],
            "After redoing the first move, ");
    }

    [TestMethod]
    public void GameEngine_RedoesMove_FailsIfNotInProgress()
    {
        var engine = CreateGameEngine();
        Assert.ThrowsException<InvalidOperationException>(() => engine.Redo());
    }

    [TestMethod]
    public void GameEngine_MakesMove_LastMoveEndsGame_Lost()
    {
        var engine = CreateGameEngine();
        engine.StartGame();
        engine.GameBoard.Nodes.SetState([0, 13, 14]);
        var move = engine.GetValidMoves().Single();
        engine.MakeMove(move);
        engine.GameBoard.Nodes.CheckState([0, 12], "After the final move, ");

        Assert.AreEqual(Stage.Lost, engine.Stage,
            $"After losing, state should be lost, but the game engine thought it was {engine.Stage}");
        Assert.IsFalse(engine.CanUndo, "After losing shouldn't be able to undo, but the game engine thought you could");
        Assert.IsFalse(engine.CanRedo, "After losing shouldn't be able to redo, but the game engine thought you could");
        Assert.IsNotNull(engine.EndTime, "After losing, game engine should set end time, but it didn't");
    }

    [TestMethod]
    public void GameEngine_MakesMove_LastMoveEndsGame_Won()
    {
        var engine = CreateGameEngine();
        engine.StartGame();
        engine.GameBoard.Nodes.SetState([13, 14]);
        var move = engine.GetValidMoves().Single();
        engine.MakeMove(move);
        engine.GameBoard.Nodes.CheckState([12], "After the final move, ");

        Assert.AreEqual(Stage.Won, engine.Stage,
            $"After losing, state should be won, but the game engine thought it was {engine.Stage}");
        Assert.IsFalse(engine.CanUndo,
            "After winning shouldn't be able to undo, but the game engine thought you could");
        Assert.IsFalse(engine.CanRedo,
            "After winning shouldn't be able to redo, but the game engine thought you could");
        Assert.IsNotNull(engine.EndTime, "After winning, game engine should set end time, but it didn't");
    }

    private GameEngine CreateGameEngine()
    {
        INodeSetup nodeSetup = new TriangularNodeSetup(new TriangularNodeAdjacencySetup());
        return new GameEngine(new GameBoard(nodeSetup.GetInitalNodeState(5).ToList()), new SimpleStateAnalyzer());
    }
}