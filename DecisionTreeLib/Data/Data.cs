namespace DecisionTreeLib.Data;

public class Data<T>(T value) : IData<T>
{
    public T Value { get; set; } = value;
}