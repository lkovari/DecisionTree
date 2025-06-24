using DecisionTreeLib.Adapters;
using DecisionTreeLib.Node;
using DecisionTreeLib.Request;

namespace DecisionTreeLib.Processing;

public interface IProcessor<T> {
    void Process(INode<T> node, IRequest<T> request);
}
