using System.Diagnostics;
using System.Numerics;

namespace PegSolitaire.Engine.GameState;

/// <summary>
/// Represents a Peg Solitaire game node
/// </summary>
/// <param name="x">The X-coordinate of the game node</param>
/// <param name="y">The Y-coordinate of the game node</param>
[DebuggerDisplay("X: {X,nq}, Y: {Y,nq}, {Occupied}")]
public class GameNode(float x, float y)
{
    /// <summary>
    /// Gets the X-coordinate of the game node
    /// </summary>
    public float X { get; } = x;

    /// <summary>
    /// Gets the Y-coordinate of the game node
    /// </summary>
    public float Y { get; } = y;

    /// <summary>
    /// Gets the game nodes adjacent to this node
    /// </summary>
    public HashSet<GameNode> Adjacent { get; } = new();

    /// <summary>
    /// Gets or sets whether there is a peg present in this node
    /// </summary>
    public bool Occupied { get; set; } = true;

    private const float Tolerance = 0.0005f;

    /// <summary>
    /// Determines whether a given node is collinear with a series of
    /// other nodes
    /// </summary>
    /// <param name="nextNodes">The series of other nodes to check</param>
    /// <returns>True if all the nodes are collinear; false if
    /// they aren't</returns>
    public bool IsCollinear(params GameNode[] nextNodes)
    {
        if (nextNodes.Length <= 1)
            return true;

        var first = new Vector2(nextNodes[0].X, nextNodes[0].Y) - new Vector2(X, Y);
        var second = new Vector2(nextNodes[1].X, nextNodes[1].Y) - new Vector2(nextNodes[0].X, nextNodes[0].Y);
        return Math.Abs(first.X * second.Y - first.Y * second.X) < Tolerance &&
               nextNodes[0].IsCollinear(nextNodes[1..]);
    }

    protected bool Equals(GameNode other)
    {
        return Math.Abs(X - other.X) < Tolerance && Math.Abs(Y - other.Y) < Tolerance;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((GameNode)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GetHashBucket(X), GetHashBucket(Y));
    }

    private static long GetHashBucket(float v)
    {
        return (long)Math.Round(v / Tolerance);
    }

    public static bool operator ==(GameNode? left, GameNode? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(GameNode? left, GameNode? right)
    {
        return !Equals(left, right);
    }
}