using DecisionTreeLib.Adapters;
using DecisionTreeLib.Node;
using DecisionTreeLib.Response;

namespace DecisionTreeLib.Evaluator;

public class DecisionTreeEvaluator
{
    private readonly IAdapter? _adapter;

    public DecisionTreeEvaluator(IAdapter? adapter)
    {
        _adapter = adapter;
    }

    public IResponse<TResult> Evaluate<TLeft, TRight, TResult>(INode<TLeft, TRight, TResult> node, IResponse<TResult>? parentResult = null)
    {
        _adapter?.Write($"Evaluating {node.Title}");
        
        return node.Execute(this, parentResult);
    }

    internal void WriteToAdapter(string message)
    {
        _adapter?.Write(message);
    }
}