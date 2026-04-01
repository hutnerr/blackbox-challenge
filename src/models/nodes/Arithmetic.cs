using Nodes;

public class AddNode : AbstractAst
{
    public AddNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public AddNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class SubtractNode : AbstractAst
{
    public SubtractNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public SubtractNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class MultiplyNode : AbstractAst
{
    public MultiplyNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public MultiplyNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class DivideNode : AbstractAst
{
    public DivideNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public DivideNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class ModuloNode : AbstractAst
{
    public ModuloNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public ModuloNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class ExponentNode : AbstractAst
{
    public ExponentNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public ExponentNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class NegationNode : AbstractAstUnary
{
    public NegationNode(IAstNode child, int startIndex, int endIndex) : base(child, startIndex, endIndex) { }
    public NegationNode(IAstNode child, Tuple<int, int> indicies) : base(child, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}


