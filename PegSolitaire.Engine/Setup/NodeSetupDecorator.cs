using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Setup;

public abstract class NodeSetupDecorator : DecoratedNodeSetup
{
    private readonly INodeSetup _NodeSetup;
    private readonly DecoratedNodeSetup? _DecoratedNodeSetup;

    public NodeSetupDecorator(INodeSetup nodeSetup)
    {
        _NodeSetup = nodeSetup;
        _DecoratedNodeSetup = nodeSetup as DecoratedNodeSetup;
    }

    public override List<GameNode> GetInitalNodeState(INodeSetup nodeSetup, int size)
    {
        if (_DecoratedNodeSetup != null)
            return _DecoratedNodeSetup.GetInitalNodeState(nodeSetup, size);
        return _NodeSetup.GetInitalNodeState(size);
    }
}