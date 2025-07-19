using PegSolitaire.Engine.GameState;
using PegSolitaire.Engine.Setup;
using PegSolitaire.Engine.Setup.Orthogonal;

namespace PegSolitaire.Engine.Tests.Setup.Orthogonal;

[TestClass]
public class SquareNodeAdjacencySetupTests
{
    [TestMethod]
    public void SquareGameBoard_HasCorrectNodeConnectionCounts()
    {
        INodeSetup setup = new SquareNodeSetup(new SquareNodeAdjacencySetup());
        for (var i = TestConstants.MinBoardSize; i <= TestConstants.MaxBoardSize; i++)
        {
            var gameNodes = setup.GetInitalNodeState(i);
            TestNodeAdjacencyCounts(gameNodes);
        }

        void TestNodeAdjacencyCounts(List<GameNode> nodes)
        {
            var maxRow = nodes.Max(n => n.Y);
            var maxColumn = nodes.Max(n => n.X);
            foreach (var node in nodes)
            {
                if (IsTopBottomEdge(node) && IsSideEdge(node))
                    AssertExpectedAdjacentCount(node, 2);
                else if (IsTopBottomEdge(node) || IsSideEdge(node))
                    AssertExpectedAdjacentCount(node, 3);
                else
                    AssertExpectedAdjacentCount(node, 4);
            }

            void AssertExpectedAdjacentCount(GameNode node, int expectedAdjacentCount) =>
                Assert.AreEqual(expectedAdjacentCount, node.Adjacent.Count,
                    $"Expected {expectedAdjacentCount} adjacent nodes to the node at X: {node.X}, Y: {node.Y}; the node has {node.Adjacent.Count}");

            //Test for bottom edge nodes using GameNode's tolerance
            bool IsTopBottomEdge(GameNode gameNode) => gameNode == new GameNode(gameNode.X, 0) ||
                                                       gameNode == new GameNode(gameNode.X, maxRow);
            bool IsSideEdge(GameNode gameNode) => gameNode == new GameNode(0, gameNode.Y) ||
                                                       gameNode == new GameNode(maxColumn, gameNode.Y);
        }
    }
}