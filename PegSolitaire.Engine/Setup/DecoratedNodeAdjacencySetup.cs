using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Setup;

/// <inheritdoc />
public abstract class DecoratedNodeAdjacencySetup : INodeAdjacencySetup
{

    /// <inheritdoc />
    public void ConnectNodes(List<GameNode> nodes) => ConnectNodes(this, nodes);

    /// <param name="nodeAdjacencySetup">The <see cref="INodeAdjacencySetup"/>
    /// to use for public interface calls</param>
    /// <inheritdoc cref="INodeAdjacencySetup.ConnectNodes" />
    public abstract void ConnectNodes(INodeAdjacencySetup nodeAdjacencySetup, List<GameNode> nodes);
}