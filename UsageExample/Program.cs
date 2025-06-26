// See https://aka.ms/new-console-template for more information

using DecisionTreeLib.Adapters;
using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Node;
using DecisionTreeLib.Processing;
using DecisionTreeLib.Request;

Console.WriteLine("Hello, Decision Tree Usage Example!");

var request = new Request<double>
{
    Operands =
    {
        ["A"] = new Data<double>(52),
        ["B"] = new Data<double>(2),
        ["C"] = new Data<double>(24),
        ["D"] = new Data<double>(8),
        ["E"] = new Data<double>(2)
    }
};

var adapter = new ConsoleAdapter();

var yesEndNode = new EndNode<double>();
var noEndNode = new EndNode<double>();

var decisionNode = new DecisionNode<double>("Is300", "FinalResult", 300, RelationType.Equal,
    yesEndNode, noEndNode);

var multiplyFinalNode = new ProcessNode<double>("FinalResult", "SubResult", "MulInnerResult", OperatorType.Multiply,
    decisionNode);

var multiplyInnerNode = new ProcessNode<double>("MulInnerResult", "DivResult", "E", OperatorType.Multiply,
    multiplyFinalNode);

var divideNode = new ProcessNode<double>("DivResult", "C", "D", OperatorType.Divide,
    multiplyInnerNode);

var subtractNode = new ProcessNode<double>("SubResult", "A", "B", OperatorType.Subtract,
    divideNode);

var processor = new DecisionTreeEvaluator<double>(adapter);
processor.Evaluate(subtractNode, request);