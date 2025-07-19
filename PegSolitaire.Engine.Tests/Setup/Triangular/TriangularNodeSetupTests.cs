using PegSolitaire.Engine.Setup;
using PegSolitaire.Engine.Setup.Triangular;

namespace PegSolitaire.Engine.Tests.Setup.Triangular;

[TestClass]
public sealed class TriangularNodeSetupTests
{
    [TestMethod]
    public void TriangularNodeSetup_HasCorrectNumberOfNodes()
    {
        INodeSetup setup = new TriangularNodeSetup(new TriangularNodeAdjacencySetup());
        for (var i = TestConstants.MinBoardSize; i <= TestConstants.MaxBoardSize; i++)
        {
            var expectedNodeCount = i * (i + 1) / 2;
            var gameNodes = setup.GetInitalNodeState(i);
            Assert.AreEqual(expectedNodeCount, gameNodes.Count,
                $"Expected {expectedNodeCount} nodes in the triangular game board; the board has {gameNodes.Count} instead");
        }
    }

    [TestMethod]
    public void TriangularNodeSetup_HasCorrectNumberOfOccupiedNodes()
    {
        INodeSetup setup = new TriangularNodeSetup(new TriangularNodeAdjacencySetup());
        for (var i = TestConstants.MinBoardSize; i <= TestConstants.MaxBoardSize; i++)
            NodeSetupTests.HasCorrectNumberOfOccupiedNodes(setup.GetInitalNodeState(i));
    }
}