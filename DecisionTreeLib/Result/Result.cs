namespace DecisionTreeLib.Result;

public class Result<T> : IResult<T>
{
    public T Value { get; set; }
}