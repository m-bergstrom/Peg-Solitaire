using PegSolitaire.Engine.Analysis;
using PegSolitaire.Engine.GameState;
using PegSolitaire.Engine.Setup;
using PegSolitaire.Engine.Setup.Orthogonal;
using PegSolitaire.Engine.Setup.Triangular;

namespace PegSolitaire.Engine.Tests.Analysis;

[TestClass]
public class SimpleStateAnalyzerTests
{
    [TestMethod]
    public void SimpleStateAnalyzer_ContinuesGame()
    {
        var analyzer = new SimpleStateAnalyzer();
        var gameBoards = CreateContinueGameBoards();
        foreach (var gameBoard in gameBoards)
        {
            var stage = analyzer.Analyze(gameBoard);
            Assert.AreEqual(Stage.InProgress, stage,
                $"Expected a game in progress but the {nameof(SimpleStateAnalyzer)} thinks it should be {stage}");
        }
    }

    [TestMethod]
    public void SimpleStateAnalyzer_EndsGame_Win()
    {
        var analyzer = new SimpleStateAnalyzer();
        var gameBoards = CreateWinGameBoards();

        foreach (var gameBoard in gameBoards)
        {
            var stage = analyzer.Analyze(gameBoard);
            Assert.AreEqual(Stage.Won, stage,
                $"Expected a game won but the {nameof(SimpleStateAnalyzer)} thinks it should be {stage}");
        }
    }

    [TestMethod]
    public void SimpleStateAnalyzer_EndsGame_Loss()
    {
        var analyzer = new SimpleStateAnalyzer();
        var gameBoards = CreateLossGameBoards();

        foreach (var gameBoard in gameBoards)
        {
            var stage = analyzer.Analyze(gameBoard);
            Assert.AreEqual(Stage.Lost, stage,
                $"Expected a game lost but the {nameof(SimpleStateAnalyzer)} thinks it should be {stage}");
        }
    }

    private List<GameBoard> CreateContinueGameBoards()
    {
        var triangularBoard = new GameBoard(
            ((INodeSetup)new TriangularNodeSetup(new TriangularNodeAdjacencySetup())).GetInitalNodeState(5));
        var squareBoard =
            new GameBoard(((INodeSetup)new SquareNodeSetup(new SquareNodeAdjacencySetup())).GetInitalNodeState(5));

        //Simulate "loss" conditions
        triangularBoard.Nodes.SetState([7, 8, 11, 13, 14]);
        squareBoard.Nodes.SetState([10, 13, 14]);
        return
        [
            triangularBoard,
            squareBoard
        ];
    }

    private List<GameBoard> CreateLossGameBoards()
    {
        var triangularBoard = new GameBoard(
            ((INodeSetup)new TriangularNodeSetup(new TriangularNodeAdjacencySetup())).GetInitalNodeState(5));
        var squareBoard =
            new GameBoard(((INodeSetup)new SquareNodeSetup(new SquareNodeAdjacencySetup())).GetInitalNodeState(5));

        //Simulate "loss" conditions
        triangularBoard.Nodes.SetState([1,2]);
        squareBoard.Nodes.SetState([11, 13]);
        return
        [
            triangularBoard,
            squareBoard
        ];
    }

    private List<GameBoard> CreateWinGameBoards()
    {
        var triangularBoard = new GameBoard(
            ((INodeSetup)new TriangularNodeSetup(new TriangularNodeAdjacencySetup())).GetInitalNodeState(5));
        var squareBoard =
            new GameBoard(((INodeSetup)new SquareNodeSetup(new SquareNodeAdjacencySetup())).GetInitalNodeState(5));

        //Simulate "win" conditions
        triangularBoard.Nodes.SetState([4]);
        squareBoard.Nodes.SetState([12]);
        return
        [
            triangularBoard,
            squareBoard
        ];
    }
}