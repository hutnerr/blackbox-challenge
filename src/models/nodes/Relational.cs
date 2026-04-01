using Nodes;

public class EqualsNode : AbstractAst
{
    public EqualsNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public EqualsNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class NotEqualsNode : AbstractAst
{
    public NotEqualsNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public NotEqualsNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class LessThanNode : AbstractAst
{
    public LessThanNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public LessThanNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class LessThanEqualToNode : AbstractAst
{
    public LessThanEqualToNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public LessThanEqualToNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class GreaterThanNode : AbstractAst
{
    public GreaterThanNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public GreaterThanNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class GreaterThanEqualToNode : AbstractAst
{
    public GreaterThanEqualToNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public GreaterThanEqualToNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}