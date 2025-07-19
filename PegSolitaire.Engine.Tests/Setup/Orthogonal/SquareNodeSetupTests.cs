using PegSolitaire.Engine.Setup;
using PegSolitaire.Engine.Setup.Orthogonal;

namespace PegSolitaire.Engine.Tests.Setup.Orthogonal;

[TestClass]
public sealed class SquareNodeSetupTests
{
    [TestMethod]
    public void SquareNodeSetup_HasCorrectNumberOfNodes()
    {
        INodeSetup setup = new SquareNodeSetup(new SquareNodeAdjacencySetup());
        for (var i = TestConstants.MinBoardSize; i <= TestConstants.MaxBoardSize; i++)
        {
            var expectedNodeCount = i * i;
            var gameNodes = setup.GetInitalNodeState(i);
            Assert.AreEqual(expectedNodeCount, gameNodes.Count,
                $"Expected {expectedNodeCount} nodes in the square game board; the board has {gameNodes.Count} instead");
        }
    }

    [TestMethod]
    public void SquareNodeSetup_HasCorrectNumberOfOccupiedNodes()
    {
        INodeSetup setup = new SquareNodeSetup(new SquareNodeAdjacencySetup());
        for (var i = TestConstants.MinBoardSize; i <= TestConstants.MaxBoardSize; i++)
            NodeSetupTests.HasCorrectNumberOfOccupiedNodes(setup.GetInitalNodeState(i));
    }
}