namespace DecisionTreeLibApi.Models;

public class EvaluationResponse
{
    public string Title { get; set; } = string.Empty;
    public object? Value { get; set; }
    public string Message { get; set; } = string.Empty;
}

