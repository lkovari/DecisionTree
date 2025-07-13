Decision Tree basic implementation with tiny usage example and tests.
(draft version)
- arithmetic operations covered by CalculationNode
- standard relations covered by DecisionNode

TODOs:
- implement more sophisticated tests, for example complex mixed decision tree
- pass the result to next node or pass it to global storage and the next node can get the parent node result
- fix warnings
- Better exception usage for example at Validators the InvalidOperationException not exactly cover the fact
- implement LogicalNode to doing logical operation between proper data types
- get a rid of the switch in evaluator create execution for each node type
- eliminate dynamic and switch in evaluator (The built-in arithmetic operations of .NET (e.g., +, -) only work generically between identical types or with explicit conversion.)
- create builder for nodes
- 
Completed:
- get a rid of the switch in evaluator create execution for each node type