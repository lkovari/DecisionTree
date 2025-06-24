// See https://aka.ms/new-console-template for more information

using DecisionTreeLib.Adapters;
using DecisionTreeLib.Data;
using DecisionTreeLib.Enums;
using DecisionTreeLib.Node;
using DecisionTreeLib.Processing;
using DecisionTreeLib.Request;

Console.WriteLine("Hello, Decision Tree Usage Example!");

var request = new Request<double>();
request.Operands["A"] = new Data<double> { Value = 52 };
request.Operands["B"] = new Data<double> { Value = 2 };
request.Operands["C"] = new Data<double> { Value = 24 };
request.Operands["D"] = new Data<double> { Value = 8 };
request.Operands["E"] = new Data<double> { Value = 2 };

var adapter = new ConsoleAdapter<double>();

var subtract = new ProcessNode<double>("SubResult", "A", "B", OperatorType.Subtract);
var divide = new ProcessNode<double>("DivResult", "C", "D", OperatorType.Divide);
var multiplyInner = new ProcessNode<double>("MulInnerResult", "DivResult", "E", OperatorType.Multiply);
var multiplyFinal = new ProcessNode<double>("FinalResult", "SubResult", "MulInnerResult", OperatorType.Multiply);
var decision = new DecisionNode<double>("Is300", "FinalResult", 300, RelationType.Equal);

subtract.NextNode = divide;
divide.NextNode = multiplyInner;
multiplyInner.NextNode = multiplyFinal;
multiplyFinal.NextNode = decision;

var processor = new Processor<double>(adapter);
processor.Process(subtract, request);