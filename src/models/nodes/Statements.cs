using Nodes;
using System.Collections.Generic;

public class PrintNode : AbstractAstUnary
{
    public PrintNode(IAstNode child, int startIndex, int endIndex) : base(child, startIndex, endIndex) { }
    public PrintNode(IAstNode child, Tuple<int, int> indicies) : base(child, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class AssignmentNode : AbstractAst
{
    // FIXME: if left is swapped to type string then the constructor gets confused. 
    // The left child probably doesn't need to be a node. It has to be a
    // string, but it's never going to get evaluated and do other node-like
    // things.
    // FIXME: maybe ensure that it is a variable node?
    public AssignmentNode(IAstNode left, IAstNode right, int startIndex, int endIndex) : base(left, right, startIndex, endIndex) { }
    public AssignmentNode(IAstNode left, IAstNode right, Tuple<int, int> indicies) : base(left, right, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class BlockNode : IAstNode
{
    private List<IAstNode> statements;
    private int startIndex;
    private int endIndex;

    public BlockNode(List<IAstNode> statements, int startIndex, int endIndex)
    {
        this.statements = statements;
        this.startIndex = startIndex;
        this.endIndex = endIndex;
    }

    public BlockNode(List<IAstNode> statements, Tuple<int, int> indicies) : this(statements, indicies.Item1, indicies.Item2)
    {
    }

    public R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }

    public List<IAstNode> GetStatements() { return statements; }

    public Tuple<int, int> getIndicies() { return Tuple.Create(startIndex, endIndex); }
}

public class ProgramNode : IAstNode
{
    private List<IAstNode> statements;
    private int startIndex;
    private int endIndex;
    public ProgramNode(List<IAstNode> statements, int startIndex, int endIndex)
    {
        this.statements = statements;
        this.startIndex = startIndex;
        this.endIndex = endIndex;
    }

    public R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }

    public List<IAstNode> GetStatements() { return statements; }
    
    public Tuple<int, int> getIndicies() { return Tuple.Create(startIndex, endIndex); }
}
