using DecisionTreeLib.Result;

namespace DecisionTreeLib.Response;

public interface IResponse<T>
{
    IResult<T>? Result { get; set; }
}