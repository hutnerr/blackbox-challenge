using Nodes;

class Translator : IVisitor<string>
{
    // -------------- PRIMITIVES --------------
    public string Visit(IntegerPrimitive node)
    {
        return node.getValue().ToString();
    }

    public string Visit(FloatPrimitive node)
    {
        return node.getValue().ToString();
    }

    public string Visit(BooleanPrimitive node)
    {
        return node.getValue().ToString();
    }

    public string Visit(StringPrimitive node)
    {
        return node.getValue();
    }

    public string Visit(NullPrimitive node)
    {
        return "null";
    }

    // -------------- ARITHMETIC --------------
    public string Visit(AddNode node)
    {
        return $"({node.getLeft().Visit(this)} + {node.getRight().Visit(this)})";
    }

    public string Visit(SubtractNode node)
    {
        return $"({node.getLeft().Visit(this)} - {node.getRight().Visit(this)})";
    }

    public string Visit(MultiplyNode node)
    {
        return $"({node.getLeft().Visit(this)} * {node.getRight().Visit(this)})";
    }

    public string Visit(DivideNode node)
    {
        return $"({node.getLeft().Visit(this)} / {node.getRight().Visit(this)})";
    }
    
    public string Visit(ModuloNode node)
    {
        return $"({node.getLeft().Visit(this)} % {node.getRight().Visit(this)})";
    }

    public string Visit(ExponentNode node)
    {
        return $"({node.getLeft().Visit(this)} ** {node.getRight().Visit(this)})";
    }

    public string Visit(NegationNode node)
    {
        return $"-({node.getChild().Visit(this)})";
    }

    // -------------- LOGICAL --------------
    public string Visit(AndNode node)
    {
        return $"({node.getLeft().Visit(this)} && {node.getRight().Visit(this)})";
    }

    public string Visit(OrNode node)
    {
        return $"({node.getLeft().Visit(this)} || {node.getRight().Visit(this)})";
    }

    public string Visit(NotNode node)
    {
        return $"!({node.getChild().Visit(this)})";
    }

    // -------------- BITWISE --------------

    public string Visit(BitAndNode node)
    {
        return $"({node.getLeft().Visit(this)} & {node.getRight().Visit(this)})";
    }

    public string Visit(BitOrNode node)
    {
        return $"({node.getLeft().Visit(this)} | {node.getRight().Visit(this)})";
    }

    public string Visit(BitXorNode node)
    {
        return $"({node.getLeft().Visit(this)} ^ {node.getRight().Visit(this)})";
    }

    public string Visit(BitNotNode node)
    {
        return $"~({node.getChild().Visit(this)})";
    }

    public string Visit(BitLeftShiftNode node)
    {
        return $"({node.getLeft().Visit(this)} << {node.getRight().Visit(this)})";
    }

    public string Visit(BitRightShiftNode node)
    {
        return $"({node.getLeft().Visit(this)} >> {node.getRight().Visit(this)})";
    }

    // -------------- RELATIONAL --------------

    public string Visit(EqualsNode node)
    {
        return $"({node.getLeft().Visit(this)} == {node.getRight().Visit(this)})";
    }

    public string Visit(NotEqualsNode node)
    {
        return $"({node.getLeft().Visit(this)} != {node.getRight().Visit(this)})";
    }

    public string Visit(LessThanNode node)
    {
        return $"({node.getLeft().Visit(this)} < {node.getRight().Visit(this)})";
    }

    public string Visit(LessThanEqualToNode node)
    {
        return $"({node.getLeft().Visit(this)} <= {node.getRight().Visit(this)})";
    }

    public string Visit(GreaterThanNode node)
    {
        return $"({node.getLeft().Visit(this)} > {node.getRight().Visit(this)})";
    }

    public string Visit(GreaterThanEqualToNode node)
    {
        return $"({node.getLeft().Visit(this)} >= {node.getRight().Visit(this)})";
    }

    // -------------- CASTING --------------
    public string Visit(FloatToInt node)
    {
        return $"int({node.getChild().Visit(this)})";
    }

    public string Visit(IntToFloat node)
    {
        return $"float({node.getChild().Visit(this)})";
    }

    // -------------- STATEMENTS --------------
    public string Visit(PrintNode node)
    {
        return $"print({node.getChild().Visit(this)})";
    }

    public string Visit(AssignmentNode node)
    {
        return $"{node.getLeft().Visit(this)} = {node.getRight().Visit(this)}";
    }

    public string Visit(BlockNode node)
    {
        string result = "{\n";
        foreach (IAstNode statement in node.GetStatements())
        {
            result += "\t" + statement.Visit(this) + "\n";
        }
        result += "}";
        return result;
    }

    public string Visit(ProgramNode node)
    {
        string result = "";
        foreach (IAstNode statement in node.GetStatements())
        {
            result += statement.Visit(this) + "\n";
        }
        return result;
    }

    // -------------- VARIABLES --------------
    public string Visit(VariableNode node)
    {
        return node.getName();
    }

}
