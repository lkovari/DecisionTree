// See https://aka.ms/new-console-template for more information

using DecisionTreeLib.Result;

using DecisionTreeLib.Adapters;
using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Node;
using DecisionTreeLib.Processing;
using DecisionTreeLib.Request;
using DecisionTreeLib.Response;

Console.WriteLine("Hello, Decision Tree Usage Example!");
const int ExpectedResult = 300;
// (52 - 2) * (24 / 8) * 2

// Első művelet: 52 - 2
var left1 = new Data<int>(52);
var right1 = new Data<int>(2);
var request1 = new OperationRequest<int, int>(left1, right1, OperatorType.Subtract);

// Második művelet: 24 / 8
var left2 = new Data<int>(24);
var right2 = new Data<int>(8);
var request2 = new OperationRequest<int, int>(left2, right2, OperatorType.Divide);

// Harmadik művelet: eredmény1 * eredmény2
var left3 = new Data<int>(50); // ideális esetben ezek a korábbi eredmények lennének
var right3 = new Data<int>(3);
var request3 = new OperationRequest<int, int>(left3, right3, OperatorType.Multiply);

// Negyedik művelet: eredmény3 * 2
var left4 = new Data<int>(150);
var right4 = new Data<int>(2);
var request4 = new OperationRequest<int, int>(left4, right4, OperatorType.Multiply);

var left5 = new Data<int>(ExpectedResult);
var right5 = new Data<int>(300);
var request5 = new DecisionRequest<int, int>(left5, right5, RelationType.Equal);

var yesEndNode = new EndNode<int, int, int>(
    "Yes End",
    new Response<int> { Title = "Yes", Result = new DecisionTreeLib.Result.Result<int> { Value = 300 } }
);
var noEndNode = new EndNode<int, int, int>(
    "No End",
    new Response<int> { Title = "No", Result = new DecisionTreeLib.Result.Result<int> { Value = 0 } }
);

var processNode5 = new DecisionNode<int, int, int>("Is it 300", request5, yesEndNode, noEndNode);
var processNode4 = new ProcessNode<int, int, int>("Multiply by 2", request4, processNode5);
var processNode3 = new ProcessNode<int, int, int>("Multiply", request3, processNode4);
var processNode2 = new ProcessNode<int, int, int>("Divide", request2, processNode3);
var processNode1 = new ProcessNode<int, int, int>("Subtract", request1, processNode2);

var adapter = new ConsoleAdapter();
var evaluator = new DecisionTreeEvaluator(adapter);
evaluator.Evaluate<int, int, int>(processNode1);