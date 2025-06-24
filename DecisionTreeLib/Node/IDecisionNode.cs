using DecisionTreeLib.Relation;
using DecisionTreeLib.Request;
using DecisionTreeLib.Response;

namespace DecisionTreeLib.Node;

public interface IDecisionNode<T> : INode<T>, IRelation {
    INode<T> YesNextNode { get; set; }
    INode<T> NoNextNode { get; set; }
}