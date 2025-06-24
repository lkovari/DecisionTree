using DecisionTreeLib.Result;

namespace DecisionTreeLib.Response;

public class Response<T> : IResponse<T>
{
    public string Title { get; set; }
    public IResult<T>? Result { get; set; }
}