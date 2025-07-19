using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Setup.Orthogonal;

public class SquareNodeAdjacencySetup : DecoratedNodeAdjacencySetup
{
    public override void ConnectNodes(INodeAdjacencySetup nodeAdjacencySetup, List<GameNode> nodes)
    {
        var differences = new[] { -1f, 0f, 1f };
        var nodesByNode = nodes.ToDictionary(n => n, n => n);

        foreach (var node in nodes)
        {
            foreach (var yDifference in differences)
            {
                foreach (var xDifference in differences)
                {
                    if (Math.Abs(Math.Abs(xDifference) - Math.Abs(yDifference)) < 1)
                        continue;
                    if (nodesByNode.TryGetValue(new GameNode(node.X + xDifference, node.Y + yDifference),
                            out var adjacent) && node != adjacent)
                        node.AddAdjacent(adjacent);
                }
            }
        }
    }
}