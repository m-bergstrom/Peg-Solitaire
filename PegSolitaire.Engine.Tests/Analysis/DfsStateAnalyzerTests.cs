using PegSolitaire.Engine.Analysis;
using PegSolitaire.Engine.GameState;
using PegSolitaire.Engine.Setup;
using PegSolitaire.Engine.Setup.Triangular;

namespace PegSolitaire.Engine.Tests.Analysis;

[TestClass]
public class DfsStateAnalyzerTests
{
    [TestMethod]
    public void DfsStateAnalyzer_ContinuesGame()
    {
        var gameBoard = CreateGameBoard();

        //Simulate an "in-progress" condition (that could be won)
        gameBoard.Nodes.SetState([7, 8, 11, 13, 14]);

        var analyzer = new DfsStateAnalyzer();
        var stage = analyzer.Analyze(gameBoard);
        Assert.AreEqual(Stage.InProgress, stage,
            $"Expected a game in progress but the ${nameof(DfsStateAnalyzer)} thinks it should be {stage}");
    }

    [TestMethod]
    public void DfsStateAnalyzer_EndsGame_Win()
    {
        var gameBoard = CreateGameBoard();

        //Simulate a "win" condition
        gameBoard.Nodes.SetState([4]);

        var analyzer = new DfsStateAnalyzer();
        var stage = analyzer.Analyze(gameBoard);
        Assert.AreEqual(Stage.Won, stage,
            $"Expected a game won but the ${nameof(DfsStateAnalyzer)} thinks it should be {stage}");
    }

    [TestMethod]
    public void DfsStateAnalyzer_EndsGame_Loss()
    {
        var gameBoard = CreateGameBoard();

        //Simulate a "loss" condition
        gameBoard.Nodes.SetState([0, 7, 8, 10, 11, 13, 14]);

        var analyzer = new DfsStateAnalyzer();
        var stage = analyzer.Analyze(gameBoard);
        Assert.AreEqual(Stage.Lost, stage,
            $"Expected a game lost but the ${nameof(DfsStateAnalyzer)} thinks it should be {stage}");
    }

    private GameBoard CreateGameBoard()
    {
        INodeSetup nodeSetup = new TriangularNodeSetup(new TriangularNodeAdjacencySetup());
        return new GameBoard(nodeSetup.GetInitalNodeState(5).ToList());
    }
}