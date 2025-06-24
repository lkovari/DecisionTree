using DecisionTreeLib.Data;

namespace DecisionTreeLib.Adapters;

public interface IAdapter<T>
{
    void Show(IData<T> data);
}