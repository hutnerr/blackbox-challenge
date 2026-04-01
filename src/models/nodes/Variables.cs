using Nodes;

public class VariableNode : IAstNode
{
    private string name;
    private int startIndex;
    private int endIndex;

    public VariableNode(string name, int startIndex, int endIndex)
    {
        this.name = name;
        this.startIndex = startIndex;
        this.endIndex = endIndex;
    }

    public VariableNode(string name, Tuple<int, int> indicies) : this(name, indicies.Item1, indicies.Item2)
    {
    }

    public R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
    public string getName() { return name; }

    public Tuple<int, int> getIndicies() { return Tuple.Create(startIndex, endIndex); }
}
