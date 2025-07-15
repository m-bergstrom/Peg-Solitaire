using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Setup;

public static class NodeExtensions
{
    /// <summary>
    /// Creates a bilateral link between two nodes
    /// </summary>
    /// <param name="thisNode">The first node to link</param>
    /// <param name="otherNode">The second node to link</param>
    public static void AddAdjacent(this GameNode thisNode, GameNode otherNode)
    {
        if (thisNode == otherNode)
            return;
        thisNode.Adjacent.Add(otherNode);
        otherNode.Adjacent.Add(thisNode);
    }
}