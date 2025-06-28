// See https://aka.ms/new-console-template for more informatio
using DecisionTreeLib.Adapters;
using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Evaluator;
using DecisionTreeLib.Node;
using DecisionTreeLib.Request;
using DecisionTreeLib.Response;

Console.WriteLine("Hello, Decision Tree Usage Example!");
Console.WriteLine();
const int expectedResult = 300;

// (52 - 2) * (24 / 8) * 2

// 52 - 2
var left1 = new Data<int>(52);
var right1 = new Data<int>(2);
var request1 = new OperationRequest<int, int>(left1, right1, OperatorType.Subtract);

// 24 / 8
var left2 = new Data<int>(24);
var right2 = new Data<int>(8);
var request2 = new OperationRequest<int, int>(left2, right2, OperatorType.Divide);

var left3 = new Data<int>(50);
var right3 = new Data<int>(3);
var request3 = new OperationRequest<int, int>(left3, right3, OperatorType.Multiply);

// result3 * 2
var left4 = new Data<int>(150);
var right4 = new Data<int>(2);
var request4 = new OperationRequest<int, int>(left4, right4, OperatorType.Multiply);

var left5 = new Data<int>(expectedResult);
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

var calculationNode5 = new DecisionNode<int, int, int>("Decision: Is it 300", request5, yesEndNode, noEndNode);
var calculationNode4 = new CalculationNode<int, int, int>("Calculation: Multiply", request4, calculationNode5);
var calculationNode3 = new CalculationNode<int, int, int>("Calculation: Multiply", request3, calculationNode4);
var calculationNode2 = new CalculationNode<int, int, int>("Calculation: Divide", request2, calculationNode3);
var calculationNode1 = new CalculationNode<int, int, int>("Calculation: Subtract", request1, calculationNode2);

var adapter = new ConsoleAdapter();
var evaluator = new DecisionTreeEvaluator(adapter);
evaluator.Evaluate<int, int, int>(calculationNode1);