namespace PegSolitaire.Engine.GameState;

/// <summary>
/// Represents a (potential) jump
/// </summary>
/// <param name="from">The node to jump from</param>
/// <param name="over">The node to jump over</param>
/// <param name="to">The node to jump to</param>
public class Jump(GameNode from, GameNode over, GameNode to)
{
    /// <summary>
    /// Gets the node to jump from
    /// </summary>
    public GameNode From { get; } = from;

    /// <summary>
    /// Gets the node to jump over
    /// </summary>
    public GameNode Over { get; } = over;

    /// <summary>
    /// Gets the node to jump to
    /// </summary>
    public GameNode To { get; } = to;

    /// <summary>
    /// Gets whether, in the current game board state, this jump is valid
    /// </summary>
    public bool IsValid => From.Occupied && Over.Occupied && !To.Occupied;

    /// <summary>
    /// Checks whether, in the current game board state, this jump is
    /// valid
    /// </summary>
    /// <exception cref="InvalidOperationException">this jump is not
    /// valid in the current game board state</exception>
    public void CheckValidity()
    {
        if (!From.Occupied)
            throw new InvalidOperationException("The spot jumping from must not be empty");
        if (!Over.Occupied)
            throw new InvalidOperationException("The spot jumping over must not be empty");
        if (To.Occupied)
            throw new InvalidOperationException("The spot jumping to must be empty");
    }

    /// <summary>
    /// Executes the jump
    /// </summary>
    /// <param name="board">The game board to execute the jump from</param>
    /// <returns>A clone of <paramref name="board"/> altered by the
    /// jump's execution</returns>
    public GameBoard Execute(GameBoard board)
    {
        CheckValidity();
        var nextBoard = board.Clone();
        //remove the peg from the starting place
        nextBoard.IndexedNodes[From].Occupied = false;
        //remove the peg from the node jumped over
        nextBoard.IndexedNodes[Over].Occupied = false;
        //put the peg from the starting place into the new location
        nextBoard.IndexedNodes[To].Occupied = true;

        return nextBoard;
    }

    private bool Equals(Jump other)
    {
        return From.Equals(other.From) && Over.Equals(other.Over) && To.Equals(other.To);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Jump)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(From, Over, To);
    }

    public static bool operator ==(Jump? left, Jump? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Jump? left, Jump? right)
    {
        return !Equals(left, right);
    }
}