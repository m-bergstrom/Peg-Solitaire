using PegSolitaire.Engine.Analysis;
using PegSolitaire.Engine.GameState;
using PegSolitaire.Engine.Setup;
using PegSolitaire.Engine.Setup.Orthogonal;
using PegSolitaire.Engine.Setup.Triangular;

namespace PegSolitaire.Engine.Tests.Analysis;

[TestClass]
public class DfsStateAnalyzerTests
{
    [TestMethod]
    public void DfsStateAnalyzer_ContinuesGame()
    {
        var analyzer = new DfsStateAnalyzer();
        foreach (var gameBoard in CreateContinueGameBoards())
        {
            var stage = analyzer.Analyze(gameBoard);
            Assert.AreEqual(Stage.InProgress, stage,
                $"Expected a game in progress but the {nameof(DfsStateAnalyzer)} thinks it should be {stage}");
        }
    }

    [TestMethod]
    public void DfsStateAnalyzer_EndsGame_Win()
    {
        var analyzer = new DfsStateAnalyzer();
        foreach (var gameBoard in CreateWinGameBoards())
        {
            var stage = analyzer.Analyze(gameBoard);
            Assert.AreEqual(Stage.Won, stage,
                $"Expected a game won but the {nameof(DfsStateAnalyzer)} thinks it should be {stage}");
        }
    }

    [TestMethod]
    public void DfsStateAnalyzer_EndsGame_Loss()
    {
        var analyzer = new DfsStateAnalyzer();
        foreach (var gameBoard in CreateLossGameBoards())
        {
            var stage = analyzer.Analyze(gameBoard);
            Assert.AreEqual(Stage.Lost, stage,
                $"Expected a game lost but the {nameof(DfsStateAnalyzer)} thinks it should be {stage}");
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
        //squareBoard.Nodes.SetState([10, 11, 13]);
        return
        [
            triangularBoard,
            //squareBoard
        ];
    }

    private List<GameBoard> CreateLossGameBoards()
    {
        var triangularBoard = new GameBoard(
            ((INodeSetup)new TriangularNodeSetup(new TriangularNodeAdjacencySetup())).GetInitalNodeState(5));
        var squareBoard =
            new GameBoard(((INodeSetup)new SquareNodeSetup(new SquareNodeAdjacencySetup())).GetInitalNodeState(5));

        //Simulate "loss" conditions
        triangularBoard.Nodes.SetState([0, 7, 8, 10, 11, 13, 14]);
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