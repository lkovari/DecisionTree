namespace DecisionTreeLib.Result;

public class Result<T> : IResult<T>
{
    public T Value { get; set; }
    public Result(T value) => Value = value;
}