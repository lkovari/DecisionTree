using DecisionTreeLib.Enums;

namespace DecisionTreeLib.Relation;

public class Relation : IRelation
{
    private RelationType Type { get; set; }

    RelationType IRelation.Relation
    {
        get => Type;
        set => Type = value;
    }

    public Relation(RelationType type)
    {
        Type = type;
    }
}