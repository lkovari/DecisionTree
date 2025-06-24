using DecisionTreeLib.Result;

namespace DecisionTreeLib.Response;

public interface IResponse<T>
{
    public string Title { get; set; }
    IResult<T>? Result { get; set; }
}