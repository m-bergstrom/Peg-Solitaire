using Microsoft.Extensions.DependencyInjection;
using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Setup.Triangular;

public class TriangularNodeSetup : DecoratedNodeSetup
{
    private INodeAdjacencySetup _NodeAdjacencySetup;

    public TriangularNodeSetup([FromKeyedServices(StringConstants.BoardShapes.Triangular)]INodeAdjacencySetup nodeAdjacencySetup)
    {
        _NodeAdjacencySetup = nodeAdjacencySetup;
    }

    private const float DistanceBetweenColumns = 0.8660254037844386f; // Math.Sqrt(3) / 2


    /// <inheritdoc />
    /// <summary>Sets up initial <see cref="GameNode"/> graph for a
    /// triangular <see cref="GameBoard"/></summary>
    public override IEnumerable<GameNode> GetInitalNodeState(INodeSetup nodeSetup, int size)
    {
        var firstRow = new HashSet<GameNode> { new(0, 0) { Occupied = false } };

        var nodes = GetFollowingRows(firstRow, firstRow.ToList(), size - 1).ToList();

        _NodeAdjacencySetup.ConnectNodes(nodes);

        return nodes;
    }

    private static HashSet<GameNode> GetFollowingRows(HashSet<GameNode> allRows, List<GameNode> lastRow, int size)
    {
        if (size < 0)
            throw new ArgumentException("Cannot create fewer than 0 rows", nameof(size));

        if (size == 0)
            return allRows;

        var thisRow = new HashSet<GameNode>();

        foreach (var lastRowNode in lastRow)
        {
            thisRow.Add(new GameNode(lastRowNode.X - DistanceBetweenColumns, lastRowNode.Y + 1));
            thisRow.Add(new GameNode(lastRowNode.X + DistanceBetweenColumns, lastRowNode.Y + 1));
        }

        return GetFollowingRows(new HashSet<GameNode>(allRows.Union(thisRow)), thisRow.ToList(), size - 1);
    }
}