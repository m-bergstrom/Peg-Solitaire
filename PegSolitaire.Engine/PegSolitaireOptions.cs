namespace PegSolitaire.Engine;

public class PegSolitaireOptions
{
    /// <summary>
    /// Gets whether to use an advanced state analyzer to halt a game
    /// early when it becomes impossible to win
    /// </summary>
    public bool UseAdvancedStateAnalyzer { get; set; }
}