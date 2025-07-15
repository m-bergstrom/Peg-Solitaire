namespace PegSolitaire.Engine.GameState;

/// <summary>
/// The base class for different shapes of Peg Solitaire game boards.
/// </summary>
public sealed class GameBoard : ICloneable<GameBoard>
{
    /// <summary>
    /// Gets the graph of interconnected <see cref="GameNode"/>s that
    /// comprise the game board
    /// </summary>
    public List<GameNode> Nodes { get; }

    /// <summary>
    /// Gets this game board's nodes indexed by the node for easy
    /// reference from copies
    /// </summary>
    public Dictionary<GameNode, GameNode> IndexedNodes { get; }

    /// <summary>
    /// Gets all possible moves, whether valid or invalid
    /// </summary>
    public List<Jump> Jumps { get; }

    /// <summary>
    /// Gets all possible moves, whether valid or invalid, indexed by
    /// origin node
    /// </summary>
    public Dictionary<GameNode, List<Jump>> IndexedJumps { get; }

    public int NodeCount => Nodes.Count;
    public int JumpCount => Jumps.Count;

    public GameBoard(List<GameNode> nodes)
    {
        Nodes = nodes;
        IndexedNodes = Nodes.ToDictionary(n => n, n => n);
        Jumps = FindJumps(Nodes).ToList();
        IndexedJumps = Jumps.GroupBy(j => j.From).ToDictionary(g => g.Key, g => g.ToList());
    }

    private GameBoard(GameBoard board)
    {
        //Create copies of the board's Nodes
        var nodePairsByOriginals =
            board.Nodes.ToDictionary(n => n, n => new GameNode(n.X, n.Y) { Occupied = n.Occupied });
        //Copy node adjacency
        foreach (var nodePair in nodePairsByOriginals)
        {
            var original = nodePair.Key;
            var clone = nodePair.Value;
            foreach (var originalAdjacent in original.Adjacent)
                clone.Adjacent.Add(nodePairsByOriginals[originalAdjacent]);
        }

        Nodes = nodePairsByOriginals.Values.ToList();
        IndexedNodes = Nodes.ToDictionary(n => n, n => n);

        //Create copies of the board's Jumps
        Jumps = board.Jumps.Select(j =>
            new Jump(nodePairsByOriginals[j.From], nodePairsByOriginals[j.Over], nodePairsByOriginals[j.To])).ToList();
        IndexedJumps = Jumps.GroupBy(j => j.From).ToDictionary(g => g.Key, g => g.ToList());
    }

    private IEnumerable<Jump> FindJumps(List<GameNode> nodes)
    {
        foreach (var node in nodes)
        {
            foreach (var adjacent in node.Adjacent)
            {
                foreach (var potentialTarget in adjacent.Adjacent)
                {
                    if (potentialTarget != node && node.IsCollinear(adjacent, potentialTarget))
                        yield return new Jump(node, adjacent, potentialTarget);
                }
            }
        }
    }

    public GameBoard Clone()
    {
        return new GameBoard(this);
    }
}