using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Setup;


/// <summary>
/// Sets up adjacency connections between nodes
/// </summary>
public interface INodeAdjacencySetup
{
    /// <summary>
    /// Sets up adjacency connections between nodes
    /// </summary>
    /// <param name="nodes">The nodes to connect</param>
    void ConnectNodes(List<GameNode> nodes);
}