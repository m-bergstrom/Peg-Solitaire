using PegSolitaire.Engine.GameState;

namespace PegSolitaire.Engine.Analysis;

public class StateAnalyzerDecorator : DecoratedStateAnalyzer
{
    private IStateAnalyzer _StateAnalyzer;
    private readonly DecoratedStateAnalyzer? _DecoratedStateAnalyzer;

    public StateAnalyzerDecorator(IStateAnalyzer stateAnalyzer)
    {
        _StateAnalyzer = stateAnalyzer;
        _DecoratedStateAnalyzer = stateAnalyzer as DecoratedStateAnalyzer;
    }

    public override Stage Analyze(IStateAnalyzer stateAnalyzer, GameBoard board)
    {
        if (_DecoratedStateAnalyzer != null)
            return _DecoratedStateAnalyzer.Analyze(stateAnalyzer, board);
        return _StateAnalyzer.Analyze(board);
    }
}