using DecisionTreeLib.Response;
using DecisionTreeLib.Evaluator;

namespace DecisionTreeLib.Node;

public interface INode<TLeft, TRight, TResult>
{
    Guid NodeId { get; }
    string Title { get; }
    Dictionary<Guid, IResponse<TResult>> ResultMap { get; set; }
    IResponse<TResult> Execute(DecisionTreeEvaluator evaluator, IResponse<TResult>? parentResult = null);
}