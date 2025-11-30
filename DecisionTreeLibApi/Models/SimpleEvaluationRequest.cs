namespace DecisionTreeLibApi.Models;

public class SimpleEvaluationRequest
{
    public int LeftValue { get; set; }
    public int RightValue { get; set; }
    public string Operation { get; set; } = string.Empty;
    public string? Relation { get; set; }
    public int? ExpectedValue { get; set; }
}

