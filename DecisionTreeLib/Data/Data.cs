namespace DecisionTreeLib.Data;

public class Data<T> : IData<T>
{
    public T Value { get; set; }
    public Data(T value) => Value = value;
}