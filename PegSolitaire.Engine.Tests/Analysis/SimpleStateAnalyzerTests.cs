using PegSolitaire.Engine.Analysis;
using PegSolitaire.Engine.GameState;
using PegSolitaire.Engine.Setup;
using PegSolitaire.Engine.Setup.Triangular;

namespace PegSolitaire.Engine.Tests.Analysis;

[TestClass]
public class SimpleStateAnalyzerTests
{
    [TestMethod]
    public void SimpleStateAnalyzer_ContinuesGame()
    {
        var gameBoard = CreateGameBoard();
        var analyzer = new SimpleStateAnalyzer();
        var stage = analyzer.Analyze(gameBoard);
        Assert.AreEqual(Stage.InProgress, stage,
            $"Expected a game in progress but the ${nameof(SimpleStateAnalyzer)} thinks it should be {stage}");
    }

    [TestMethod]
    public void SimpleStateAnalyzer_EndsGame_Win()
    {
        var gameBoard = CreateGameBoard();

        //Simulate a "win" condition
        for (var i = 0; i < gameBoard.Nodes.Count - 1; i++)
            gameBoard.Nodes[i].Occupied = false;
        var analyzer = new SimpleStateAnalyzer();
        var stage = analyzer.Analyze(gameBoard);
        Assert.AreEqual(Stage.Won, stage,
            $"Expected a game won but the ${nameof(SimpleStateAnalyzer)} thinks it should be {stage}");
    }

    [TestMethod]
    public void SimpleStateAnalyzer_EndsGame_Loss()
    {
        var gameBoard = CreateGameBoard();

        //Simulate a "loss" condition
        for (var i = 2; i < gameBoard.Nodes.Count - 1; i++)
            gameBoard.Nodes[i].Occupied = false;
        var analyzer = new SimpleStateAnalyzer();
        var stage = analyzer.Analyze(gameBoard);
        Assert.AreEqual(Stage.Lost, stage,
            $"Expected a game lost but the ${nameof(SimpleStateAnalyzer)} thinks it should be {stage}");
    }

    private GameBoard CreateGameBoard()
    {
        INodeSetup nodeSetup = new TriangularNodeSetup(new TriangularNodeAdjacencySetup());
        return new GameBoard(nodeSetup.GetInitalNodeState(5).ToList());
    }
}