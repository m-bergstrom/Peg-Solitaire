using PegSolitaire.Engine.GameState;
using PegSolitaire.Engine.Setup;
using PegSolitaire.Engine.Setup.Triangular;

namespace PegSolitaire.Engine.Tests.Setup.Triangular;

[TestClass]
public class TriangularNodeAdjacencySetupTests
{
    [TestMethod]
    public void TriangularGameBoard_HasCorrectNodeConnectionCounts()
    {
        INodeSetup setup = new TriangularNodeSetup(new TriangularNodeAdjacencySetup());
        for (var i = TestConstants.MinBoardSize; i <= TestConstants.MaxBoardSize; i++)
        {
            var gameNodes = setup.GetInitalNodeState(i);
            TestNodeAdjacencyCounts(gameNodes);
        }

        void TestNodeAdjacencyCounts(List<GameNode> nodes)
        {
            var rows = nodes.GroupBy(n => n.Y).ToDictionary(r => r.Key, r => r.ToList());
            foreach (var rowIndex in rows.Keys)
            {
                var row = rows[rowIndex];

                if (rowIndex == 0)
                {
                    //Check that there's only one node in the first row
                    Assert.AreEqual(1, row.Count);
                    //Check that the node in the first row is adjacent to 2 others
                    Assert.AreEqual(2, row[0].Adjacent.Count,
                        $"Expected {2} adjacent nodes to the node at X: {row[0].X}, Y: {row[0].Y}; the node has {row[0].Adjacent.Count}");
                    continue;
                }

                //Find out the left and right side edge positions
                var minX = row.Min(n => n.X);
                var maxX = row.Max(n => n.X);

                foreach (var node in row)
                {
                    //Nodes in the bottom left and right corners
                    //should be adjacent to 2 other nodes
                    if (IsBottomEdge(node) && IsSideEdge(node))
                        AssertExpectedAdjacentCount(node, 2);
                    //Nodes at the edges (but not the corners) should
                    //be adjacent to 4 other nodes
                    else if (IsBottomEdge(node) || IsSideEdge(node))
                        AssertExpectedAdjacentCount(node, 4);
                    //All nodes not on the edges or in the corners
                    //should be adjacent to 6 other nodes
                    else
                        AssertExpectedAdjacentCount(node, 6);
                }

                continue;

                void AssertExpectedAdjacentCount(GameNode node, int expectedAdjacentCount) =>
                    Assert.AreEqual(expectedAdjacentCount, node.Adjacent.Count,
                        $"Expected {expectedAdjacentCount} adjacent nodes to the node at X: {node.X}, Y: {node.Y}; the node has {node.Adjacent.Count}");

                //Test for bottom edge nodes using GameNode's tolerance
                bool IsBottomEdge(GameNode gameNode) => gameNode == new GameNode(gameNode.X, rows.Count - 1);

                //Test for side edge nodes using GameNode's tolerance
                bool IsSideEdge(GameNode gameNode) => gameNode == new GameNode(minX, gameNode.Y) ||
                                                      gameNode == new GameNode(maxX, gameNode.Y);
            }
        }
    }
}