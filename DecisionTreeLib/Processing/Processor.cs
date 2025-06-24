using DecisionTreeLib.Adapters;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Node;
using DecisionTreeLib.Request;

namespace DecisionTreeLib.Processing;

public class Processor<T>
{
    private readonly IAdapter<T> _adapter;

    public Processor(IAdapter<T> adapter)
    {
        _adapter = adapter;
    }

    public void Process(INode<T> node, IRequest<T> request)
    {
        switch (node)
        {
            case ProcessNode<T> processNode:
            {
                dynamic left = request.Operands[processNode.LeftOperandKey].Value;
                dynamic right = request.Operands[processNode.RightOperandKey].Value;

                dynamic resultValue = processNode.Operator switch
                {
                    OperatorType.Add => left + right,
                    OperatorType.Subtract => left - right,
                    OperatorType.Multiply => left * right,
                    OperatorType.Divide => left / right,
                    _ => throw new InvalidOperationException()
                };

                var result = new DecisionTreeLib.Result.Result<T> { Value = resultValue };
                var response = new DecisionTreeLib.Response.Response<T> { Result = result };

                if (!processNode.TypedResultMaps.TryGetValue(typeof(T), out var mapObj))
                {
                    // mapObj = new Dictionary<Guid, IResponse<T>>();
                    // processNode.TypedResultMaps[typeof(T)] = mapObj;
                    processNode.AddResult(processNode.NodeId, response);
                }

                //var map = (Dictionary<Guid, IResponse<T>>)mapObj;
                //map[processNode.NodeId] = response;

                request.Operands[processNode.Title] = new DecisionTreeLib.Data.Data<T>(resultValue);
                _adapter.Write($"{processNode.Title} = {resultValue}");

                if (processNode.NextNode != null)
                    Process(processNode.NextNode, request);
                break;
            }
            case DecisionNode<T> decisionNode:
            {
                dynamic value = request.Operands[decisionNode.OperandKey].Value;
                bool condition = decisionNode.RelationType switch
                {
                    RelationType.Equal => value == (dynamic)decisionNode.CompareValue,
                    RelationType.GreaterThan => value > (dynamic)decisionNode.CompareValue,
                    RelationType.LessThan => value < (dynamic)decisionNode.CompareValue,
                    RelationType.GreaterThanOrEqual => value >= (dynamic)decisionNode.CompareValue,
                    RelationType.LessThanOrEqual => value <= (dynamic)decisionNode.CompareValue,
                    RelationType.NotEqual => value != (dynamic)decisionNode.CompareValue,
                    _ => throw new ArgumentOutOfRangeException(nameof(decisionNode.RelationType))
                };

                var result = new DecisionTreeLib.Result.Result<T> { Value = value };
                var response = new DecisionTreeLib.Response.Response<T> { Result = result };
                if (!decisionNode.TypedResultMaps.TryGetValue(typeof(T), out var mapObj))
                {
                    //mapObj = new Dictionary<Guid, IResponse<T>>();
                    //decisionNode.TypedResultMaps[typeof(T)] = mapObj;
                    decisionNode.AddResult(decisionNode.NodeId, response);
                }

                //var map = (Dictionary<Guid, IResponse<T>>)mapObj;
                //map[decisionNode.NodeId] = response;
                _adapter.Write($"{decisionNode.Title}: {condition}");

                if (condition && decisionNode.YesNextNode != null)
                    Process(decisionNode.YesNextNode, request);
                else if (!condition && decisionNode.NoNextNode != null)
                    Process(decisionNode.NoNextNode, request);
                break;
            }
            case EndNode<T> endNode:
                Console.WriteLine($"End of Processing: {endNode.Title}");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(node), $"Unknown Node type: {node.GetType().Name}");
        }
    }
}