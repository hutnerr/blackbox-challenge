using Nodes;

// an assignment node returns primtive value of its right hand side
// a print node returns null
// a block returns the primitive value of its last statement, or a null primitive if it has no statements

public class Evaluator : IVisitor<IPrimitiveNode>
{
    Runtime Env;

    public Evaluator(Runtime env)
    {
        Env = env;
    }

    // helper function to determine the new indicies of a binary operation
    // take the min of the start indicies and the max of the end indicies
    private Tuple<int, int> determineNewIndicies(IAstNode left, IAstNode right)
    {
        Tuple<int, int> leftIndicies = left.getIndicies();
        Tuple<int, int> rightIndicies = right.getIndicies();
        int startIndex = Math.Min(leftIndicies.Item1, rightIndicies.Item1);
        int endIndex = Math.Max(leftIndicies.Item2, rightIndicies.Item2);
        return Tuple.Create(startIndex, endIndex);
    }

    // -------------- PRIMITIVES --------------
    public IPrimitiveNode Visit(IntegerPrimitive node)
    {
        return node;
    }

    public IPrimitiveNode Visit(FloatPrimitive node)
    {
        return node;
    }

    public IPrimitiveNode Visit(BooleanPrimitive node)
    {
        return node;
    }

    public IPrimitiveNode Visit(StringPrimitive node)
    {
        return node;
    }

    public IPrimitiveNode Visit(NullPrimitive node)
    {
        return node;
    }

    // -------------- ARITHMETIC --------------
    public IPrimitiveNode Visit(AddNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);

        if (!(left is IntegerPrimitive || left is FloatPrimitive))
        {
            throw new InvalidOperationException($"Addition requires numeric primitives, got {left.GetType().Name} on left");
        }

        IPrimitiveNode right = node.getRight().Visit(this);

        if (!(right is IntegerPrimitive || right is FloatPrimitive))
        {
            throw new InvalidOperationException($"Addition requires numeric primitives, got {right.GetType().Name} on right");
        }

        if (left.GetType() != right.GetType())
        {
            throw new InvalidOperationException($"Cannot add two different numeric types, got {left.GetType().Name} on left, and {right.GetType().Name} on right");
        }

        if (left is IntegerPrimitive leftInt && right is IntegerPrimitive rightInt)
        {
            return new IntegerPrimitive(leftInt.getValue() + rightInt.getValue(), determineNewIndicies(left, right));
        }
        else
        {
            return new FloatPrimitive(((FloatPrimitive)left).getValue() + ((FloatPrimitive)right).getValue(), determineNewIndicies(left, right));
        }
        throw new InvalidOperationException("Addition failed: Unknown reason");
    }

    public IPrimitiveNode Visit(SubtractNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);

        if (!(left is IntegerPrimitive || left is FloatPrimitive))
        {
            throw new InvalidOperationException($"Subtraction requires numeric primitives, got {left.GetType().Name} on left");
        }

        IPrimitiveNode right = node.getRight().Visit(this);

        if (!(right is IntegerPrimitive || right is FloatPrimitive))
        {
            throw new InvalidOperationException($"Subtraction requires numeric primitives, got {right.GetType().Name} on right");
        }

        if (left.GetType() != right.GetType())
        {
            throw new InvalidOperationException($"Cannot subtract two different numeric types, got {left.GetType().Name} on left, and {right.GetType().Name} on right");
        }

        if (left is IntegerPrimitive leftInt && right is IntegerPrimitive rightInt)
        {
            return new IntegerPrimitive(leftInt.getValue() - rightInt.getValue(), determineNewIndicies(left, right));
        }
        else if (left is FloatPrimitive leftFloat && right is FloatPrimitive rightFloat)
        {
            return new FloatPrimitive(leftFloat.getValue() - rightFloat.getValue(), determineNewIndicies(left, right));
        }
        throw new InvalidOperationException("Subtraction failed: Unknown reason");
    }

    public IPrimitiveNode Visit(MultiplyNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);

        if (!(left is IntegerPrimitive || left is FloatPrimitive))
        {
            throw new InvalidOperationException($"Multiplication requires numeric primitives, got {left.GetType().Name} on left");
        }

        IPrimitiveNode right = node.getRight().Visit(this);

        if (!(right is IntegerPrimitive || right is FloatPrimitive))
        {
            throw new InvalidOperationException($"Multiplication requires numeric primitives, got {right.GetType().Name} on right");
        }

        if (left.GetType() != right.GetType())
        {
            throw new InvalidOperationException($"Cannot multiply two different numeric types, got {left.GetType().Name} on left, and {right.GetType().Name} on right");
        }

        if (left is IntegerPrimitive leftInt && right is IntegerPrimitive rightInt)
        {
            return new IntegerPrimitive(leftInt.getValue() * rightInt.getValue(), determineNewIndicies(left, right));
        }
        else if (left is FloatPrimitive leftFloat && right is FloatPrimitive rightFloat)
        {
            return new FloatPrimitive(leftFloat.getValue() * rightFloat.getValue(), determineNewIndicies(left, right));
        }
        throw new InvalidOperationException("Multiplication failed: Unknown reason");
    }

public IPrimitiveNode Visit(DivideNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);

        if (!(left is IntegerPrimitive || left is FloatPrimitive))
        {
            throw new InvalidOperationException($"Division requires numeric primitives, got {left.GetType().Name} on left");
        }

        IPrimitiveNode right = node.getRight().Visit(this);

        if (!(right is IntegerPrimitive || right is FloatPrimitive))
        {
            throw new InvalidOperationException($"Division requires numeric primitives, got {right.GetType().Name} on right");
        }

        if (right is IntegerPrimitive rightIntCheck)
        {
            if (rightIntCheck.getValue() == 0)
            {
                throw new DivideByZeroException("Division by zero is not allowed.");
            }
        }
        
        if (right is FloatPrimitive rightFloatCheck)
        {
            if (rightFloatCheck.getValue() == 0)
            {
                throw new DivideByZeroException("Division by zero.");
            }
        }

        if (left is IntegerPrimitive leftInt && right is IntegerPrimitive rightInt)
        {
            return new IntegerPrimitive(leftInt.getValue() / rightInt.getValue(), determineNewIndicies(left, right));
        }
        else if (left is FloatPrimitive leftFloat && right is FloatPrimitive rightFloat)
        {
            return new FloatPrimitive(leftFloat.getValue() / rightFloat.getValue(), determineNewIndicies(left, right));
        }
        else if (left is FloatPrimitive leftFloat1 && right is IntegerPrimitive rightInt1)
        {
            return new FloatPrimitive(leftFloat1.getValue() / rightInt1.getValue(), determineNewIndicies(left, right));
        }
        else if (left is IntegerPrimitive leftInt1 && right is FloatPrimitive rightFloat1)
        {
            return new FloatPrimitive(leftInt1.getValue() / rightFloat1.getValue(), determineNewIndicies(left, right));
        }
        throw new InvalidOperationException("Division failed: Unknown reason");
    }

    public IPrimitiveNode Visit(ModuloNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);

        if (!(left is IntegerPrimitive || left is FloatPrimitive))
        {
            throw new InvalidOperationException($"Modulo requires numeric primitives, got {left.GetType().Name} on left");
        }

        IPrimitiveNode right = node.getRight().Visit(this);

        if (!(right is IntegerPrimitive || right is FloatPrimitive))
        {
            throw new InvalidOperationException($"Modulo requires numeric primitives, got {right.GetType().Name} on right");
        }

        if (left.GetType() != right.GetType())
        {
            throw new InvalidOperationException($"Cannot mod two different numeric types, got {left.GetType().Name} on left, and {right.GetType().Name} on right");
        }

        if (left is IntegerPrimitive leftInt && right is IntegerPrimitive rightInt)
        {
            return new IntegerPrimitive(leftInt.getValue() % rightInt.getValue(), determineNewIndicies(left, right));
        }
        else if (left is FloatPrimitive leftFloat && right is FloatPrimitive rightFloat)
        {
            return new FloatPrimitive(leftFloat.getValue() % rightFloat.getValue(), determineNewIndicies(left, right));
        }
        throw new InvalidOperationException("Modulo failed: Unknown reason");
    }

    public IPrimitiveNode Visit(ExponentNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);

        if (!(left is IntegerPrimitive || left is FloatPrimitive))
        {
            throw new InvalidOperationException($"Exponentiation requires numeric primitives, got {left.GetType().Name} on left");
        }

        IPrimitiveNode right = node.getRight().Visit(this);

        if (!(right is IntegerPrimitive || right is FloatPrimitive))
        {
            throw new InvalidOperationException($"Exponentiation requires numeric primitives, got {right.GetType().Name} on right");
        }

        if (left.GetType() != right.GetType())
        {
            throw new InvalidOperationException($"Cannot exponentiatiate two different numeric types, got {left.GetType().Name} on left, and {right.GetType().Name} on right");
        }

        if (left is IntegerPrimitive leftInt && right is IntegerPrimitive rightInt)
        {
            return new FloatPrimitive(Math.Pow(leftInt.getValue(), rightInt.getValue()), determineNewIndicies(left, right));
        }
        else if (left is FloatPrimitive leftFloat && right is FloatPrimitive rightFloat)
        {
            return new FloatPrimitive(Math.Pow(leftFloat.getValue(), leftFloat.getValue()), determineNewIndicies(left, right));
        }
        throw new InvalidOperationException("Modulo failed: Unknown reason");
    }

    public IPrimitiveNode Visit(NegationNode node)
    {
        IPrimitiveNode child = node.getChild().Visit(this);

        if (!(child is IntegerPrimitive || child is FloatPrimitive))
        {
            throw new InvalidOperationException($"Negation requires numeric primitives, got {child.GetType().Name}");
        }

        if (child is IntegerPrimitive intVal)
        {
            return new IntegerPrimitive(-intVal.getValue(), node.getIndicies());
        }
        else if (child is FloatPrimitive floatVal)
        {
            return new FloatPrimitive(-floatVal.getValue(), node.getIndicies());
        }
        throw new InvalidOperationException("Negation failed: Unknown reason");
    }

    // -------------- LOGICAL --------------
    public IPrimitiveNode Visit(AndNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);

        if (!(left is BooleanPrimitive))
        {
            throw new InvalidOperationException($"AND requires boolean primitives, got {left.GetType().Name} on left");
        }

        BooleanPrimitive leftBool = (BooleanPrimitive)left;

        // leave early if left is false
        if (!leftBool.getValue())
        {
            return new BooleanPrimitive(false, left.getIndicies());
        }

        IPrimitiveNode right = node.getRight().Visit(this);

        if (!(right is BooleanPrimitive))
        {
            throw new InvalidOperationException($"AND requires boolean primitives, got {right.GetType().Name} on right");
        }

        BooleanPrimitive rightBool = (BooleanPrimitive)right;
        return new BooleanPrimitive(leftBool.getValue() && rightBool.getValue(), determineNewIndicies(left, right));
    }

    public IPrimitiveNode Visit(OrNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);

        if (!(left is BooleanPrimitive))
        {
            throw new InvalidOperationException($"OR requires boolean primitives, got {left.GetType().Name} on left");
        }

        BooleanPrimitive leftBool = (BooleanPrimitive)left;

        // leave early if left is true
        if (leftBool.getValue())
        {
            return new BooleanPrimitive(true, left.getIndicies());
        }

        IPrimitiveNode right = node.getRight().Visit(this);

        if (!(right is BooleanPrimitive))
        {
            throw new InvalidOperationException($"OR requires boolean primitives, got {right.GetType().Name} on right");
        }

        BooleanPrimitive rightBool = (BooleanPrimitive)right;
        return new BooleanPrimitive(leftBool.getValue() || rightBool.getValue(), determineNewIndicies(left, right));
    }

    public IPrimitiveNode Visit(NotNode node)
    {
        IPrimitiveNode child = node.getChild().Visit(this);

        if (!(child is BooleanPrimitive))
        {
            throw new InvalidOperationException($"NOT requires boolean primitive, got {child.GetType().Name}");
        }

        BooleanPrimitive boolVal = (BooleanPrimitive)child;
        return new BooleanPrimitive(!boolVal.getValue(), node.getIndicies());
    }

    // -------------- BITWISE --------------

    public IPrimitiveNode Visit(BitAndNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);

        if (!(left is IntegerPrimitive))
        {
            throw new InvalidOperationException($"Bitwise AND requires integer primitives, got {left.GetType().Name} on left");
        }

        IPrimitiveNode right = node.getRight().Visit(this);

        if (!(right is IntegerPrimitive))
        {
            throw new InvalidOperationException($"Bitwise AND requires integer primitives, got {right.GetType().Name} on right");
        }

        IntegerPrimitive leftInt = (IntegerPrimitive)left;
        IntegerPrimitive rightInt = (IntegerPrimitive)right;
        return new IntegerPrimitive(leftInt.getValue() & rightInt.getValue(), determineNewIndicies(left, right));
    }

    public IPrimitiveNode Visit(BitOrNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);

        if (!(left is IntegerPrimitive))
        {
            throw new InvalidOperationException($"Bitwise OR requires integer primitives, got {left.GetType().Name} on left");
        }

        IPrimitiveNode right = node.getRight().Visit(this);

        if (!(right is IntegerPrimitive))
        {
            throw new InvalidOperationException($"Bitwise OR requires integer primitives, got {right.GetType().Name} on right");
        }

        IntegerPrimitive leftInt = (IntegerPrimitive)left;
        IntegerPrimitive rightInt = (IntegerPrimitive)right;
        return new IntegerPrimitive(leftInt.getValue() | rightInt.getValue(), determineNewIndicies(left, right));
    }

    public IPrimitiveNode Visit(BitXorNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);

        if (!(left is IntegerPrimitive))
        {
            throw new InvalidOperationException($"Bitwise XOR requires integer primitives, got {left.GetType().Name} on left");
        }

        IPrimitiveNode right = node.getRight().Visit(this);

        if (!(right is IntegerPrimitive))
        {
            throw new InvalidOperationException($"Bitwise XOR requires integer primitives, got {right.GetType().Name} on right");
        }

        IntegerPrimitive leftInt = (IntegerPrimitive)left;
        IntegerPrimitive rightInt = (IntegerPrimitive)right;
        return new IntegerPrimitive(leftInt.getValue() ^ rightInt.getValue(), determineNewIndicies(left, right));
    }

    public IPrimitiveNode Visit(BitNotNode node)
    {
        IPrimitiveNode child = node.getChild().Visit(this);

        if (!(child is IntegerPrimitive))
        {
            throw new InvalidOperationException($"Bitwise NOT requires integer primitive, got {child.GetType().Name}");
        }
        IntegerPrimitive intVal = (IntegerPrimitive)child;
        return new IntegerPrimitive(~intVal.getValue(), node.getIndicies());
    }

    public IPrimitiveNode Visit(BitLeftShiftNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);

        if (!(left is IntegerPrimitive))
        {
            throw new InvalidOperationException($"Bitwise Left Shift requires integer primitives, got {left.GetType().Name} on left");
        }

        IPrimitiveNode right = node.getRight().Visit(this);

        if (!(right is IntegerPrimitive))
        {
            throw new InvalidOperationException($"Bitwise Left Shift requires integer primitives, got {right.GetType().Name} on right");
        }

        IntegerPrimitive leftInt = (IntegerPrimitive)left;
        IntegerPrimitive rightInt = (IntegerPrimitive)right;
        return new IntegerPrimitive(leftInt.getValue() << rightInt.getValue(), determineNewIndicies(left, right));
    }

    public IPrimitiveNode Visit(BitRightShiftNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);

        if (!(left is IntegerPrimitive))
        {
            throw new InvalidOperationException($"Bitwise Right Shift requires integer primitives, got {left.GetType().Name} on left");
        }

        IPrimitiveNode right = node.getRight().Visit(this);

        if (!(right is IntegerPrimitive))
        {
            throw new InvalidOperationException($"Bitwise Right Shift requires integer primitives, got {right.GetType().Name} on right");
        }

        IntegerPrimitive leftInt = (IntegerPrimitive)left;
        IntegerPrimitive rightInt = (IntegerPrimitive)right;
        return new IntegerPrimitive(leftInt.getValue() >> rightInt.getValue(), determineNewIndicies(left, right));
    }

    // -------------- RELATIONAL --------------

    public IPrimitiveNode Visit(EqualsNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);
        IPrimitiveNode right = node.getRight().Visit(this);

        if (left.GetType() != right.GetType())
        {
            throw new InvalidOperationException($"Cannot compare two different primitive types, got {left.GetType().Name} on left and got {right.GetType().Name} on right");
        }

        if (left is IntegerPrimitive leftInt && right is IntegerPrimitive rightInt)
        {
            return new BooleanPrimitive(leftInt.getValue() == rightInt.getValue(), determineNewIndicies(left, right));
        }
        else if (left is FloatPrimitive leftFloat && right is FloatPrimitive rightFloat)
        {
            return new BooleanPrimitive(leftFloat.getValue() == rightFloat.getValue(), determineNewIndicies(left, right));
        }
        else if (left is StringPrimitive leftString && right is StringPrimitive rightString)
        {
            return new BooleanPrimitive(leftString.getValue() == rightString.getValue(), determineNewIndicies(left, right));
        }
        else if (left is BooleanPrimitive leftBool && right is BooleanPrimitive rightBool)
        {
            return new BooleanPrimitive(leftBool.getValue() == rightBool.getValue(), determineNewIndicies(left, right));
        }

        throw new InvalidOperationException($"Equality Comparison not supported for the {left.GetType().Name} type.");
    }

    public IPrimitiveNode Visit(NotEqualsNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);
        IPrimitiveNode right = node.getRight().Visit(this);

        if (left.GetType() != right.GetType())
        {
            throw new InvalidOperationException($"Cannot compare two different primitive types, got {left.GetType().Name} on left and got {right.GetType().Name} on right");
        }

        if (left is IntegerPrimitive leftInt && right is IntegerPrimitive rightInt)
        {
            return new BooleanPrimitive(leftInt.getValue() != rightInt.getValue(), determineNewIndicies(left, right));
        }
        else if (left is FloatPrimitive leftFloat && right is FloatPrimitive rightFloat)
        {
            return new BooleanPrimitive(leftFloat.getValue() != rightFloat.getValue(), determineNewIndicies(left, right));
        }
        else if (left is StringPrimitive leftString && right is StringPrimitive rightString)
        {
            return new BooleanPrimitive(leftString.getValue() != rightString.getValue(), determineNewIndicies(left, right));
        }
        else if (left is BooleanPrimitive leftBool && right is BooleanPrimitive rightBool)
        {
            return new BooleanPrimitive(leftBool.getValue() != rightBool.getValue(), determineNewIndicies(left, right));
        }

        throw new InvalidOperationException($"Not Equal Comparison not supported for the {left.GetType().Name} type.");
    }

    public IPrimitiveNode Visit(LessThanNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);
        IPrimitiveNode right = node.getRight().Visit(this);

        if (left.GetType() != right.GetType())
        {
            throw new InvalidOperationException($"Cannot compare two different primitive types, got {left.GetType().Name} on left and got {right.GetType().Name} on right");
        }

        if (left is IntegerPrimitive leftInt && right is IntegerPrimitive rightInt)
        {
            return new BooleanPrimitive(leftInt.getValue() < rightInt.getValue(), determineNewIndicies(left, right));
        }
        else if (left is FloatPrimitive leftFloat && right is FloatPrimitive rightFloat)
        {
            return new BooleanPrimitive(leftFloat.getValue() < rightFloat.getValue(), determineNewIndicies(left, right));
        }
        throw new InvalidOperationException($"Less Than Comparison not supported for the {left.GetType().Name} type.");
    }

    public IPrimitiveNode Visit(LessThanEqualToNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);
        IPrimitiveNode right = node.getRight().Visit(this);

        if (left.GetType() != right.GetType())
        {
            throw new InvalidOperationException($"Cannot compare two different primitive types, got {left.GetType().Name} on left and got {right.GetType().Name} on right");
        }

        if (left is IntegerPrimitive leftInt && right is IntegerPrimitive rightInt)
        {
            return new BooleanPrimitive(leftInt.getValue() <= rightInt.getValue(), determineNewIndicies(left, right));
        }
        else if (left is FloatPrimitive leftFloat && right is FloatPrimitive rightFloat)
        {
            return new BooleanPrimitive(leftFloat.getValue() <= rightFloat.getValue(), determineNewIndicies(left, right));
        }
        throw new InvalidOperationException($"Less Than or Equal To Comparison not supported for the {left.GetType().Name} type.");
    }

    public IPrimitiveNode Visit(GreaterThanNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);
        IPrimitiveNode right = node.getRight().Visit(this);

        if (left.GetType() != right.GetType())
        {
            throw new InvalidOperationException($"Cannot compare two different primitive types, got {left.GetType().Name} on left and got {right.GetType().Name} on right");
        }

        if (left is IntegerPrimitive leftInt && right is IntegerPrimitive rightInt)
        {
            return new BooleanPrimitive(leftInt.getValue() > rightInt.getValue(), determineNewIndicies(left, right));
        }
        else if (left is FloatPrimitive leftFloat && right is FloatPrimitive rightFloat)
        {
            return new BooleanPrimitive(leftFloat.getValue() > rightFloat.getValue(), determineNewIndicies(left, right));
        }
        throw new InvalidOperationException($"Greater Than Comparison not supported for the {left.GetType().Name} type.");
    }

    public IPrimitiveNode Visit(GreaterThanEqualToNode node)
    {
        IPrimitiveNode left = node.getLeft().Visit(this);
        IPrimitiveNode right = node.getRight().Visit(this);

        if (left.GetType() != right.GetType())
        {
            throw new InvalidOperationException($"Cannot compare two different primitive types, got {left.GetType().Name} on left and got {right.GetType().Name} on right");
        }

        if (left is IntegerPrimitive leftInt && right is IntegerPrimitive rightInt)
        {
            return new BooleanPrimitive(leftInt.getValue() >= rightInt.getValue(), determineNewIndicies(left, right));
        }
        else if (left is FloatPrimitive leftFloat && right is FloatPrimitive rightFloat)
        {
            return new BooleanPrimitive(leftFloat.getValue() >= rightFloat.getValue(), determineNewIndicies(left, right));
        }
        throw new InvalidOperationException($"Greater Than or Equal To Comparison not supported for the {left.GetType().Name} type.");
    }

    // -------------- CASTING --------------
    public IPrimitiveNode Visit(FloatToInt node)
    {
        IPrimitiveNode child = node.getChild().Visit(this);

        if (!(child is FloatPrimitive))
        {
            throw new InvalidOperationException($"Float to Int requires a Float primitive, got {child.GetType().Name}");
        }

        FloatPrimitive floatVal = (FloatPrimitive)child;
        return new IntegerPrimitive((int)floatVal.getValue(), node.getIndicies());
    }

    public IPrimitiveNode Visit(IntToFloat node)
    {
        IPrimitiveNode child = node.getChild().Visit(this);

        if (!(child is IntegerPrimitive))
        {
            throw new InvalidOperationException($"Int to Float requires an Int primitive, got {child.GetType().Name}");
        }

        IntegerPrimitive intVal = (IntegerPrimitive)child;
        return new FloatPrimitive((float)intVal.getValue(), node.getIndicies());
    }

    // -------------- STATEMENTS --------------
    public IPrimitiveNode Visit(PrintNode node)
    {
        IPrimitiveNode child = node.getChild().Visit(this);
        Console.WriteLine(child.Visit(new Translator()));
        return new NullPrimitive(node.getIndicies());
    }

    public IPrimitiveNode Visit(AssignmentNode node)
    {
        IAstNode left = node.getLeft();
        IPrimitiveNode right = node.getRight().Visit(this);

        if (!(left is VariableNode))
            {
                throw new InvalidOperationException($"Assignment must assign to a variable, got {left.GetType().Name}");
            }

        VariableNode leftVar = (VariableNode)left;
        Env.SetVariable(leftVar.getName(), right);

        return right;
    }

    public IPrimitiveNode Visit(BlockNode node)
    {
        IPrimitiveNode value = null!;

        foreach (IAstNode statement in node.GetStatements())
        {
            value = statement.Visit(this);
        }

        return value!;
    }

    public IPrimitiveNode Visit(ProgramNode node)
    {
        IPrimitiveNode value = null!;

        foreach (IAstNode statement in node.GetStatements())
        {
            value = statement.Visit(this);
        }

        return value!;
    }

    // -------------- VARIBALES --------------
    public IPrimitiveNode Visit(VariableNode node)
    {
        string variableName = node.getName();

        if (!Env.HasVariable(variableName))
        {
            return new NullPrimitive(node.getIndicies());
            // Env.SetVariable(variableName, new NullPrimitive());
        }

        IPrimitiveNode rvalue = (IPrimitiveNode)Env.GetVariable(node.getName())!;
        return rvalue!;
    }
}
