namespace DecisionTreeLib.Result;

public interface IResult<T>
{
    T Value { get; set; }
}