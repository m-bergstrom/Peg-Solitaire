using PegSolitaire.Engine.GameState;
using PegSolitaire.Engine.Setup;
using PegSolitaire.Engine.Setup.Triangular;

namespace PegSolitaire.Engine.Tests.GameState;

[TestClass]
public class GameBoardTests
{
    [TestMethod]
    public void TriangularGameBoard_HasCorrectNumberOfPotentialJumps()
    {
        INodeSetup setup = new TriangularNodeSetup(new TriangularNodeAdjacencySetup());
        for (var i = TestConstants.MinBoardSize; i <= TestConstants.MaxBoardSize; i++)
        {
            var expectedJumpCount = (i - 2) * (i - 1) * 3;
            var board = new GameBoard(setup.GetInitalNodeState(i));
            Assert.AreEqual(expectedJumpCount, board.JumpCount,
                $"Expected {expectedJumpCount} possible jumps in the triangular game board; the board has {board.JumpCount} instead");
        }
    }

    [TestMethod]
    public void TriangularGameBoard_CloneClones()
    {
        INodeSetup setup = new TriangularNodeSetup(new TriangularNodeAdjacencySetup());
        for (var i = TestConstants.MinBoardSize; i <= TestConstants.MaxBoardSize; i++)
        {
            var board = new GameBoard(setup.GetInitalNodeState(i));
            var board2 = board.Clone();
            Assert.AreEqual(board.NodeCount, board2.NodeCount,
                $"The original board has {board.NodeCount} nodes, but the clone has {board2.NodeCount}");
            Assert.AreEqual(board.JumpCount, board2.JumpCount,
                $"The original board has {board.JumpCount} nodes, but the clone has {board2.NodeCount}");

            foreach (var originalNode in board.Nodes)
            {
                Assert.IsTrue(board2.IndexedNodes.TryGetValue(originalNode, out var clonedNode),
                    $"Node X: {originalNode.X}, Y: {originalNode.Y} from the original board is missing from the clone");

                Assert.AreEqual(originalNode.Adjacent.Count, clonedNode.Adjacent.Count,
                    $"Node X: {originalNode.X}, Y: {originalNode.Y} has {originalNode.Adjacent.Count} adjacent nodes, but the cloned node has {clonedNode.Adjacent.Count}");
            }

            var differentNodes = board2.Nodes.Except(board.Nodes).Union(board.Nodes.Except(board2.Nodes));
            Assert.AreEqual(0, differentNodes.Count(),
                "The nodes from the cloned board differ from the nodes from the original");

            var differentJumps = board2.Jumps.Except(board.Jumps).Union(board.Jumps.Except(board2.Jumps));
            Assert.AreEqual(0, differentJumps.Count(),
                "The jumps from the cloned board differ from the jumps from the original");

            var differentIndexedJumpKeys = board2.IndexedJumps.Keys.Except(board.IndexedJumps.Keys)
                .Union(board.IndexedJumps.Keys.Except(board2.IndexedJumps.Keys));
            Assert.AreEqual(0, differentIndexedJumpKeys.Count(),
                "The indexed jump keys from the cloned board differ from the indexed jump keys from the original");

            var differentIndexedJumpValues = board2.IndexedJumps.Values.SelectMany(l => l)
                .Except(board.IndexedJumps.Values.SelectMany(l => l))
                .Union(board.IndexedJumps.Values.SelectMany(l => l)
                    .Except(board2.IndexedJumps.Values.SelectMany(l => l)));
            Assert.AreEqual(0, differentIndexedJumpValues.Count(),
                "The indexed jump value lists from the cloned board differ from the indexed jump value lists from the original");
        }
    }
}