using Nodes;

public class BitAndNode : AbstractAst
{
    public BitAndNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public BitAndNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class BitOrNode : AbstractAst
{
    public BitOrNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public BitOrNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class BitXorNode : AbstractAst
{
    public BitXorNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public BitXorNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class BitNotNode : AbstractAstUnary
{
    public BitNotNode(IAstNode child, int startIndex, int endIndex) : base(child, startIndex, endIndex) { }
    public BitNotNode(IAstNode child, Tuple<int, int> indicies) : base(child, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class BitLeftShiftNode : AbstractAst
{
    public BitLeftShiftNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public BitLeftShiftNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class BitRightShiftNode : AbstractAst
{
    public BitRightShiftNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public BitRightShiftNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}