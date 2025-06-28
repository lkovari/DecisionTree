using DecisionTreeLib.Result;

namespace DecisionTreeLib.Response;

public class Response<T> : IResponse<T>
{
    public required string Title { get; set; }
    public IResult<T>? Result { get; set; }
}