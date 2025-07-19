using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Setup;

/// <inheritdoc />
public abstract class DecoratedNodeSetup : INodeSetup
{
    /// <inheritdoc />
    public List<GameNode> GetInitalNodeState(int size) => GetInitalNodeState(this, size);

    /// <param name="nodeSetup">The <see cref="INodeSetup"/> to use for public interface calls</param>
    /// <param name="size"></param>
    /// <inheritdoc cref="INodeSetup.GetInitalNodeState" />
    public abstract List<GameNode> GetInitalNodeState(INodeSetup nodeSetup, int size);
}