using Microsoft.Extensions.DependencyInjection;
using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Setup.Orthogonal;

public class SquareNodeSetup : DecoratedNodeSetup
{
    private INodeAdjacencySetup _NodeAdjacencySetup;

    public SquareNodeSetup(
        [FromKeyedServices(StringConstants.BoardShapes.Square)]
        INodeAdjacencySetup nodeAdjacencySetup)
    {
        _NodeAdjacencySetup = nodeAdjacencySetup;
    }

    public override List<GameNode> GetInitalNodeState(INodeSetup nodeSetup, int size)
    {
        var nodes = new List<GameNode>();
        for (var i = 0; i < size; i++)
        {
            for (var j = 0; j < size; j++)
            {
                var node = new GameNode(i, j)
                {
                    Occupied = !(size % 2 == 1 && i * 2 == size - 1 && j * 2 == size - 1 ||
                               size % 2 == 0 && i == 0 && j == 0)
                };
                nodes.Add(node);
            }
        }

        _NodeAdjacencySetup.ConnectNodes(nodes);

        return nodes;
    }
}