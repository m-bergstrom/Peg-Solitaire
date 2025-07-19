using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Tests.Setup;

static class NodeSetupTests
{
    public static void HasCorrectNumberOfOccupiedNodes(List<GameNode> nodes)
    {
        var occupied = nodes.Count(n => n.Occupied);
        Assert.AreEqual(nodes.Count - 1, occupied,
            $"Expected {nodes.Count - 1} occupied nodes; the board has {occupied}");
    }
}