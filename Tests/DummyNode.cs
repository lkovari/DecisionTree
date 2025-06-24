using DecisionTreeLib.Node;
using DecisionTreeLib.Request;
using DecisionTreeLib.Response;

namespace Tests;

public class DummyNode<T> : INode<T>
{
    public Guid NodeId { get; set; } = Guid.NewGuid();
    public string Title { get; set; }
    private readonly Action _onProcess;
    public Dictionary<Guid, IResponse<T>> ResultMap { get; set; } = new();

    public DummyNode(Action onProcess, string title)
    {
        _onProcess = onProcess;
        Title = title;
    }

    public void Process(IRequest<T> request)
    {
        _onProcess?.Invoke();
    }
}