namespace DecisionTreeLib.Result;

public class Result<T> : IResult<T>
{
    public required T Value { get; set; }
}