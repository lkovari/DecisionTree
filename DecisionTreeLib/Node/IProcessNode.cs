using DecisionTreeLib.Enums;
using DecisionTreeLib.Node;
using DecisionTreeLib.Response;

public interface IProcessNode<T> : INode<T>
{
    string Title { get; set; }
    string LeftOperandKey { get; }
    string RightOperandKey { get; }
    OperatorType Operator { get; }
    INode<T> NextNode { get; set; }
    Dictionary<Guid, IResponse<T>> ResultMap { get; set; }
}