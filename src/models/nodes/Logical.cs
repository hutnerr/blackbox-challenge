using Nodes;

public class AndNode : AbstractAst
{
    public AndNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public AndNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class OrNode : AbstractAst
{
    public OrNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public OrNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class NotNode : AbstractAstUnary
{
    public NotNode(IAstNode child, int startIndex, int endIndex) : base(child, startIndex, endIndex) { }
    public NotNode(IAstNode child, Tuple<int, int> indicies) : base(child, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}