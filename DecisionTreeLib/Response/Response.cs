using DecisionTreeLib.Result;

namespace DecisionTreeLib.Response;

public class Response<T> : IResponse<T>
{
    public IResult<T>? Result { get; set; }
}