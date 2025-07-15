using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Setup;

public abstract class NodeAdjacencySetupDecorator : DecoratedNodeAdjacencySetup
{
    private readonly INodeAdjacencySetup _NodeAdjacencySetup;
    private readonly DecoratedNodeAdjacencySetup? _DecoratedNodeAdjacencySetup;

    public NodeAdjacencySetupDecorator(INodeAdjacencySetup nodeAdjacencySetup)
    {
        _NodeAdjacencySetup = nodeAdjacencySetup;
        _DecoratedNodeAdjacencySetup = nodeAdjacencySetup as DecoratedNodeAdjacencySetup;
    }

    public override void ConnectNodes(INodeAdjacencySetup nodeAdjacencySetup, List<GameNode> nodes)
    {
        if (_DecoratedNodeAdjacencySetup != null)
            _DecoratedNodeAdjacencySetup.ConnectNodes(nodeAdjacencySetup, nodes);
        else
            _NodeAdjacencySetup.ConnectNodes(nodes);
    }
}