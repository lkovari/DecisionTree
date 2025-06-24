using DecisionTreeLib.Enums;

namespace DecisionTreeLib.Relation;

public interface IRelation
{
    RelationType Relation { get; set; }
}