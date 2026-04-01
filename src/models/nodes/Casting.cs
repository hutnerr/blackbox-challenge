using Nodes;

public class FloatToInt : AbstractAstUnary
{
    public FloatToInt(IAstNode child, int startIndex, int endIndex) : base(child, startIndex, endIndex) { }
    public FloatToInt(IAstNode child, Tuple<int, int> indicies) : base(child, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class IntToFloat : AbstractAstUnary
{
    public IntToFloat(IAstNode child, int startIndex, int endIndex) : base(child, startIndex, endIndex) { }
    public IntToFloat(IAstNode child, Tuple<int, int> indicies) : base(child, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}