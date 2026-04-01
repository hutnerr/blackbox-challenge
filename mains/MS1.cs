using System;
using Nodes;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.ConstrainedExecution;


class MilestoneOneTests
{
    static void MS1Main(string[] args)
    {

        Tuple<int, int> INDEX = new(0, 0);
        Evaluator evaluator = new(new Runtime());

        IntegerPrimitive zero = new(0, INDEX);
        IntegerPrimitive one = new(1, INDEX);
        IntegerPrimitive two = new(2, INDEX);
        IntegerPrimitive three = new(3, INDEX);
        IntegerPrimitive four = new(4, INDEX);
        IntegerPrimitive five = new(5, INDEX);
        IntegerPrimitive six = new(6, INDEX);
        IntegerPrimitive seven = new(7, INDEX);
        IntegerPrimitive eight = new(8, INDEX);
        IntegerPrimitive nine = new(9, INDEX);
        IntegerPrimitive seventeen = new(17, INDEX);

        // Arithmetic: (7 * 4 + 3) % 12
        ModuloNode arithmetic = new(new AddNode(new MultiplyNode(seven, four, INDEX), three, INDEX), new IntegerPrimitive(12, INDEX), INDEX);
        IPrimitiveNode arithmeticEvaluation = arithmetic.Visit(evaluator);
        Debug.Assert(((IntegerPrimitive)arithmeticEvaluation).getValue() == 7);
        Console.WriteLine(arithmetic.Visit(new Translator()));
        Console.WriteLine(arithmeticEvaluation.Visit(new Translator()));
        Console.WriteLine();

        // Arithmetic negation? (did mean multiplication?) and rvalues: a * b
        VariableNode a = new("a", INDEX);
        VariableNode b = new("b", INDEX);
        evaluator.Visit(new AssignmentNode(a, five, INDEX)); // assign a and add to memory
        evaluator.Visit(new AssignmentNode(b, two, INDEX)); // same w/ b
        MultiplyNode rvalueMultiplication = new MultiplyNode(a, b, INDEX); // since they're in memory, this should be able to be evaluated
        IPrimitiveNode rvalueMultiplicationEvaluation = rvalueMultiplication.Visit(evaluator);
        Debug.Assert(((IntegerPrimitive)rvalueMultiplicationEvaluation).getValue() == 10);
        Console.WriteLine(rvalueMultiplication.Visit(new Translator()));
        Console.WriteLine(rvalueMultiplicationEvaluation.Visit(new Translator()));
        Console.WriteLine();

        // Rvalue lookup and shift: i << 3
        VariableNode i = new("i", INDEX);
        BitLeftShiftNode rvalueLeftShift = new(new AssignmentNode(i, one, INDEX), three, INDEX);
        IPrimitiveNode rvalueLeftShiftEvaluation = rvalueLeftShift.Visit(evaluator);
        Debug.Assert(((IntegerPrimitive)rvalueLeftShiftEvaluation).getValue() == 8);
        Console.WriteLine(rvalueLeftShift.Visit(new Translator()));
        Console.WriteLine(rvalueLeftShiftEvaluation.Visit(new Translator()));
        Console.WriteLine();

        // Rvalue lookup and comparison j == j + 0
        VariableNode j = new("j", INDEX);
        evaluator.Visit(new AssignmentNode(j, five, INDEX));
        AssignmentNode rvalueLookCompare = new(j, new AddNode(j, zero, INDEX), INDEX);
        IPrimitiveNode rvalueLookCompareEvaluation = rvalueLookCompare.Visit(evaluator);
        Debug.Assert(((IntegerPrimitive)rvalueLookCompareEvaluation).getValue() == 5);
        Console.WriteLine(rvalueLookCompare.Visit(new Translator()));
        Console.WriteLine(rvalueLookCompareEvaluation.Visit(new Translator()));
        Console.WriteLine();



        // Logic and comparison: !(3.3 > 3.2)
        NotNode logicAndComparison = new(new GreaterThanNode(new FloatPrimitive(3.3, INDEX),new FloatPrimitive(3.2, INDEX), INDEX), INDEX);
        IPrimitiveNode logicAndComparisonEvaluation = logicAndComparison.Visit(evaluator);
        Debug.Assert(((BooleanPrimitive)logicAndComparisonEvaluation).getValue() == false);
        Console.WriteLine(logicAndComparison.Visit(new Translator()));
        Console.WriteLine(logicAndComparisonEvaluation.Visit(new Translator()));
        Console.WriteLine();

        // Double Negation: --(6 * 8)
        NegationNode doubleNegation = new(new NegationNode(new MultiplyNode(six, eight, INDEX), INDEX), INDEX);
        IPrimitiveNode doubleNegationEvaluation = doubleNegation.Visit(evaluator);
        Debug.Assert(((IntegerPrimitive)doubleNegationEvaluation).getValue() == 48);
        Console.WriteLine(doubleNegation.Visit(new Translator()));
        Console.WriteLine(doubleNegationEvaluation.Visit(new Translator()));
        Console.WriteLine();

        // Bitwise Operations: ~5 | ~8
        BitOrNode bitwiseOperations = new(new BitNotNode(five, INDEX), new BitNotNode(eight, INDEX), INDEX);
        IPrimitiveNode bitwiseOperationsEvaluation = bitwiseOperations.Visit(evaluator);
        Debug.Assert(((IntegerPrimitive)bitwiseOperationsEvaluation).getValue() == -1);
        Console.WriteLine(bitwiseOperations.Visit(new Translator()));
        Console.WriteLine(bitwiseOperationsEvaluation.Visit(new Translator()));
        Console.WriteLine();

        // Casting: float(7) / 2
        DivideNode divideNode = new(new IntToFloat(seven, INDEX), new FloatPrimitive(2, INDEX), INDEX);
        IPrimitiveNode divideEvalauation = divideNode.Visit(evaluator);
        Debug.Assert(((FloatPrimitive)divideEvalauation).getValue() == 3.5);
        Console.WriteLine(divideNode.Visit(new Translator()));
        Console.WriteLine(divideEvalauation.Visit(new Translator()));
        Console.WriteLine();

        // Assignment: n = 9 & 3
        AssignmentNode assignmentNode = new(new VariableNode("n", INDEX), new BitAndNode(nine, three, INDEX), INDEX);
        IPrimitiveNode assignmentEvalauation = assignmentNode.Visit(evaluator);
        Debug.Assert(((IntegerPrimitive)assignmentEvalauation).getValue() == 1);
        Console.WriteLine(assignmentNode.Visit(new Translator()));
        Console.WriteLine(assignmentEvalauation.Visit(new Translator()));
        Console.WriteLine();

        // =========== MULTIPLE STATEMENT PROGRAMS ============
        // x = 17
        // print x
        List<IAstNode> stmts1 = new List<IAstNode>();
        VariableNode x = new("x", INDEX);
        stmts1.Add(new AssignmentNode(x,seventeen, INDEX));
        stmts1.Add(new PrintNode(x, INDEX));
        BlockNode blockNode1 = new(stmts1, INDEX);
        Console.WriteLine(blockNode1.Visit(new Translator()));
        IPrimitiveNode blockEvalauation1 = blockNode1.Visit(evaluator);
        Debug.Assert(((NullPrimitive)blockEvalauation1).getValue() == null);
        // Console.WriteLine(blockEvalauation1.Visit(new Translator()));
        Console.WriteLine();

        // count = 6 << 1
        // delta = 3
        // count = count + delta
        // print count
        List<IAstNode> stmts2 = new List<IAstNode>();
        VariableNode count = new("count", INDEX);
        VariableNode delta = new("delta", INDEX);
        stmts2.Add(new AssignmentNode(count, new BitLeftShiftNode(six, one, INDEX), INDEX));
        stmts2.Add(new AssignmentNode(delta, three, INDEX));
        stmts2.Add(new AssignmentNode(count,new AddNode(count,delta, INDEX), INDEX));
        stmts2.Add(new PrintNode(count, INDEX));
        BlockNode blockNode2 = new(stmts2, INDEX);
        Console.WriteLine(blockNode2.Visit(new Translator()));
        IPrimitiveNode blockEvalauation2 = blockNode2.Visit(evaluator);
        Debug.Assert(((NullPrimitive)blockEvalauation2).getValue() == null);
        // Console.WriteLine(blockEvalauation2.Visit(new Translator()));
        Console.WriteLine();

        // n = 18
        // print n <= 18
        // print 13 <= n && n <= 16
        // print -(n ** 2)
        List<IAstNode> stmts3 = new List<IAstNode>();
        VariableNode n = new("n", INDEX);
        stmts3.Add(new AssignmentNode(n, new IntegerPrimitive(18, INDEX), INDEX));
        stmts3.Add(new PrintNode(new LessThanEqualToNode(n, new IntegerPrimitive(18, INDEX), INDEX), INDEX));
        stmts3.Add(new PrintNode(new AndNode(new LessThanEqualToNode(new IntegerPrimitive(13, INDEX), n, INDEX), new LessThanEqualToNode(n, new IntegerPrimitive(16, INDEX), INDEX), INDEX), INDEX));
        stmts3.Add(new PrintNode(new NegationNode(new ExponentNode(n, two, INDEX), INDEX), INDEX));
        BlockNode blockNode3 = new(stmts3, INDEX);
        Console.WriteLine(blockNode3.Visit(new Translator()));
        IPrimitiveNode blockEvalauation3 = blockNode3.Visit(evaluator);
        Debug.Assert(((NullPrimitive)blockEvalauation3).getValue() == null);
        // Console.WriteLine(blockEvalauation3.Visit(new Translator()));
        Console.WriteLine();

        // =========== PROGRAMS THAT FAIL TO TYPE CHECK ============


        // 7.5 << 2
        try
        {
            Console.WriteLine("Trying: 7.5 << 2");
            BitLeftShiftNode floatShiftInt = new(new FloatPrimitive(7.5, INDEX), two, INDEX);
            IPrimitiveNode floatShiftIntEvaluation = floatShiftInt.Visit(evaluator);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine();
        }

        // true >= 10
        try
        {
            Console.WriteLine("Trying: true >= 10");
            LessThanEqualToNode boolCompInt = new(new BooleanPrimitive(true, INDEX), new IntegerPrimitive(10, INDEX), INDEX);
            IPrimitiveNode boolCompIntEvaluation = boolCompInt.Visit(evaluator);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine();
        }

        // "fooo" / 3
        try
        {
            Console.WriteLine("Trying: \"foo\" / 3");
            DivideNode stringDivInt = new(new StringPrimitive("fooo", INDEX), new IntegerPrimitive(3, INDEX), INDEX);
            IPrimitiveNode stringDivIntEvaluation = stringDivInt.Visit(evaluator);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine();
        }

        // ERRORS OF OUR OWN CRAFTING

        // div by 0
        try
        {
            Console.WriteLine("Trying: 5 / 0");
            DivideNode divByZero = new(five, zero, INDEX);
            IPrimitiveNode divByZeroEvaluation = divByZero.Visit(evaluator);
        }
        catch (DivideByZeroException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine();
        }

        // variable that wasn't set yet
        try
        {
            Console.WriteLine("Trying: z + 3");
            AddNode variablePlusInt = new(new VariableNode("z", INDEX), three, INDEX);
            IPrimitiveNode variablePlusIntEvaluation = variablePlusInt.Visit(evaluator);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine();
        }

        // string variable plus an int 
        try
        {
            Console.WriteLine("Trying: z = \"stringtest\" then z + 3");
            VariableNode z = new("z", INDEX);
            AssignmentNode varAssignment = new(z, new StringPrimitive("stringtest", INDEX), INDEX);
            varAssignment.Visit(evaluator); // set our variable into memory
            AddNode stringVarPlusInt = new(z, three, INDEX);
            IPrimitiveNode stringVarPlusIntEvaluation = stringVarPlusInt.Visit(evaluator);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine();
        }
    }
}