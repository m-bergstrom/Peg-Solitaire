using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Setup.Triangular;

public class TriangularNodeAdjacencySetup : DecoratedNodeAdjacencySetup
{
    private const float DistanceBetweenColumns = 0.8660254037844386f; // Math.Sqrt(3) / 2

    public override void ConnectNodes(INodeAdjacencySetup nodeAdjacencySetup, List<GameNode> nodes)
    {
        var differences = new[] { -1f, 0f, 1f };
        var nodesByNode = nodes.ToDictionary(n => n, n => n);

        foreach (var node in nodes)
        {
            foreach (var yDifference in differences)
            {
                foreach (var xd in differences)
                {
                    if (xd == 0f)
                        continue;
                    var xDifference = xd * (yDifference == 0f ? 2f : 1f) * DistanceBetweenColumns;
                    if (nodesByNode.TryGetValue(new GameNode(node.X + xDifference, node.Y + yDifference),
                            out var adjacent) && node != adjacent)
                        node.AddAdjacent(adjacent);
                }
            }
        }
    }
}