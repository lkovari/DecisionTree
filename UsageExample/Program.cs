﻿// See https://aka.ms/new-console-template for more informatio
using DecisionTreeLib.Adapters;
using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Evaluator;
using DecisionTreeLib.Node;
using DecisionTreeLib.Request;
using DecisionTreeLib.Response;

Console.WriteLine("Decision Tree Usage Example!");
Console.WriteLine();
const int expectedResult = 300;

/*
 * (52 - 2) * (24 / 8) * 2 if result is 300 go to yes node
 * after that, execute: 300 & 200, if result is odd go to
 * "result odd", else go to "result is even"
 */

var left1 = new Data<int>(52);
var right1 = new Data<int>(2);
var request1 = new BinaryOperationRequest<int, int>
{
    LeftOperand = left1,
    RightOperand = right1,
    Operator = OperatorType.Subtract
};

var left2 = new Data<int>(24);
var right2 = new Data<int>(8);
var request2 = new BinaryOperationRequest<int, int>
{
    LeftOperand = left2,
    RightOperand = right2,
    Operator = OperatorType.Divide
};

var left3 = new Data<int>(50);
var right3 = new Data<int>(3);
var request3 = new BinaryOperationRequest<int, int>
{
    LeftOperand = left3,
    RightOperand = right3,
    Operator = OperatorType.Multiply
};

var left4 = new Data<int>(150);
var right4 = new Data<int>(2);
var request4 = new BinaryOperationRequest<int, int>
{
    LeftOperand = left4,
    RightOperand = right4,
    Operator = OperatorType.Multiply
};

var left5 = new Data<int>(expectedResult);
var right5 = new Data<int>(300);
var request5 = new DecisionRequest<int, int>(left5, right5, RelationType.Equal);

var left6 = new Data<int>(300);
var right6 = new Data<int>(200);
var request6 = new BinaryOperationRequest<int, int>
{
    LeftOperand = left6,
    RightOperand = right6,
    Operator = OperatorType.And
};

var left7 = new Data<int>(200);
var right7 = new Data<int>(1);
var request7 = new DecisionRequest<int, int>(left7, right7, RelationType.Equal);

var oddEndNode = new EndNode<int, int, int>(
    "result odd",
    new Response<int> { Title = "result odd", Result = new DecisionTreeLib.Result.Result<int> { Value = 200 } }
);
var evenEndNode = new EndNode<int, int, int>(
    "result is even",
    new Response<int> { Title = "result is even", Result = new DecisionTreeLib.Result.Result<int> { Value = 200 } }
);

var yesEndNode = new EndNode<int, int, int>(
    "Yes End",
    new Response<int> { Title = "Yes", Result = new DecisionTreeLib.Result.Result<int> { Value = 300 } }
);
var noEndNode = new EndNode<int, int, int>(
    "No End",
    new Response<int> { Title = "No", Result = new DecisionTreeLib.Result.Result<int> { Value = 0 } }
);

var oddEvenDecisionNode = new DecisionNode<int, int, int>("Decision: Is result odd?", request7, oddEndNode, evenEndNode);
var logicalAndNode = new CalculationNode<int, int, int>("Calculation: 300 & 200", request6, oddEvenDecisionNode);
var calculationNode5 = new DecisionNode<int, int, int>("Decision: Is it 300", request5, logicalAndNode, noEndNode);
var calculationNode4 = new CalculationNode<int, int, int>("Calculation: Multiply", request4, calculationNode5);
var calculationNode3 = new CalculationNode<int, int, int>("Calculation: Multiply", request3, calculationNode4);
var calculationNode2 = new CalculationNode<int, int, int>("Calculation: Divide", request2, calculationNode3);
var calculationNode1 = new CalculationNode<int, int, int>("Calculation: Subtract", request1, calculationNode2);

var adapter = new ConsoleAdapter();
var evaluator = new DecisionTreeEvaluator(adapter);
evaluator.Evaluate<int, int, int>(calculationNode1);