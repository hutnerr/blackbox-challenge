using System.Diagnostics;
using System.Net.Http.Headers;
using Nodes;
using Tokens;
using Interpreter;
using System.Runtime.CompilerServices;

// we put our tests for MS2 in here now, since MS1 is done

// we need to build an interpreter for our box coding project
// this will translate arbitrary source code entered by the user into the model classes we wrote
// we need to use state machine to emit tokens, then parse them into an AST, then evaluate the AST

class MilestoneTwoTests
{
    static void MS2Main(string[] args)
    {
        Console.WriteLine("Running Milestone 2 Tests for lexer...");
        TestLexer();
        Console.WriteLine("========================================");
        Console.WriteLine("Running Milestone 2 Tests for parser...");
        testParser();
    }

    private static void testParser()
    {
        Evaluator evaluator = new(new Runtime());

        void testCode(string code)
        {
            Lexer lexer = new Lexer(code);
            lexer.tokenize();
            Parser parser = new Parser(lexer.getTokens());
            IAstNode ast = parser.parse();
            Console.WriteLine(ast.Visit(new Translator()));
            IPrimitiveNode primitiveNode = ast.Visit(evaluator);
            switch (primitiveNode)
            {
                case IntegerPrimitive i:
                    Console.WriteLine("Evaluated to (int): " + i.getValue());
                    break;
                case FloatPrimitive f:
                    Console.WriteLine("Evaluated to (float): " + f.getValue());
                    break;
                case BooleanPrimitive b:
                    Console.WriteLine("Evaluated to (bool): " + b.getValue());
                    break;
                case StringPrimitive s:
                    Console.WriteLine("Evaluated to (string): " + s.getValue());
                    break;
                case NullPrimitive n:
                    Console.WriteLine("Evaluated to: null");
                    break;
                default:
                    Console.WriteLine("Unknown primitive type: " + primitiveNode.GetType().Name);
                    break;
            }
            Console.WriteLine("------------------------");
        }

        // ===== TEST SOME RANDOM EXAMPLES =====
        testCode("5 + 3");
        testCode("(100 / 10) * 2");
        testCode("\"Hello, world!\"");
        testCode("float ( 5 + 3 ) * 2");
        testCode("true && false || !false");
        testCode("x = 5");
        testCode("y = x * 2 + ( 10 / 5 )");
        testCode("print ( int ( 5.1 ) + .5 )");

        // ===== TEST THE EXAMPLES IN THE WRITEUP =====
        // blocks are represented by { } in our grammar
        // so there is no BLOCK token, just LEFTBRACKET and RIGHTBRACKET tokens
        // inside of these is the block itself
        string arithmeticBlock = @"{
            print ( 5 + 3 )
            print ( 10 * 6 - 10 % 4 )
            print ( ( 5 + 2 ) * 3 % 4 )
            print ( ~6 )
            print ( 2 ** 9 )
            print ( 45 & ---(1 + 3) )
            9 << 1
        }";

        string logicalBlock = @"{
            print ( 8 >= 7 + 1 )
            print ( !!!!false )
            print ( true || !false )
            print ( (5 > 3) && !(2 > 8) )
        }";

        string variablesBlock = @"{
            x = 5
            print ( x + x * x )
            x = 999
            print ( x )
        }";

        testCode(arithmeticBlock);
        testCode(logicalBlock);
        testCode(variablesBlock);

        // ===== TEST SOME ADDITIONAL CODE =====
        string additionaBlock1 = @"{
            x = 5
            y = 10
            z = x * y + ( y - x ) / 2
            print ( z )
            print ( float ( z ) / 3 )
            print ( int ( float ( z ) / 3 ) )
        }";

        string additionaBlock2 = @"{
            a = true
            b = false
            c = a || b && !a
            print ( c )
            print ( !c )
            print ( a && ( b || !a ) )
        }";

        string additionaBlock3 = @"{
            x = 40 + 2
            msg = ""The result is: ""
            num = x
            print ( msg )
            print ( num )
        }";

        string additionaBlock4 = @"{
            yes = true
            no = false
            maybe = yes && !no || ( no && yes )
            print ( maybe )
        }";

        testCode(additionaBlock1);
        testCode(additionaBlock2);
        testCode(additionaBlock3);
        testCode(additionaBlock4);

        // ===== TEST SOME INVALID CODE =====
        try { testCode("5 + 5..5"); } catch (Exception e) { Console.WriteLine(e.Message); } // invalid float
        try { testCode("5 + 5.565.5"); } catch (Exception e) { Console.WriteLine(e.Message); } // invalid float
        try { testCode("\"Hello world!..."); } catch (Exception e) { Console.WriteLine(e.Message); } // unterminated string
        try { testCode("5 $ 3"); } catch (Exception e) { Console.WriteLine(e.Message); } // $ is not a valid char
        try { testCode("print ( \"The value of x is: \" + x )"); } catch (Exception e) { Console.WriteLine(e.Message); } // we do not support string concatenation yet
        try { testCode("x = 5 + 5.0"); } catch (Exception e) { Console.WriteLine(e.Message); } // we dont support int + float
        try { testCode("true + false"); } catch (Exception e) { Console.WriteLine(e.Message); } // cant add booleans
    }

    private static void TestLexer()
    {

        void testCode(string code)
        {
            Lexer lexer = new Lexer(code);
            lexer.tokenize();
            foreach (var token in lexer.getTokens()) { Console.WriteLine(token); }
            Console.WriteLine("------------------------");
        }

        // ===== TEST SOME RANDOM EXAMPLES =====
        testCode("5 + 3");
        testCode("(100 / 10) * 2");
        testCode("\"Hello, world!\"");
        testCode("float ( 5 + 3 ) * 2");
        testCode("true && false || !false");
        testCode("x = 5");
        testCode("y = x * 2 + ( 10 / 5 )");
        testCode("print ( \"The value of x is: \" + x )");
        testCode("print ( float ( 5 ) + .5 )");

        // ===== TEST THE EXAMPLES IN THE WRITEUP =====
        // blocks are represented by { } in our grammar
        // so there is no BLOCK token, just LEFTBRACKET and RIGHTBRACKET tokens
        // inside of these is the block itself
        string arithmeticBlock = @"{
            print ( 5 + 3 )
            print ( 10 * 6 - 10 % 4 )
            print ( ( 5 + 2 ) * 3 % 4 )
            print ( ~6 )
            print ( 2 ** 9 )
            print ( 45 & ---(1 + 3) )
            9 << 1
        }";

        string logicalBlock = @"{
            print ( 8 >= 7 + 1 )
            print ( !!!!false )
            print ( true || !false )
            print ( (5 > 3) && !(2 > 8) )
        }";

        string variablesBlock = @"{
            x = 5
            print ( x + x * x )
            x = 999
            print ( x )
        }";

        testCode(arithmeticBlock);
        testCode(logicalBlock);
        testCode(variablesBlock);

        // ===== TEST SOME INVALID CODE =====
        try { testCode("5 + 5..5"); } catch (Exception e) { Console.WriteLine(e.Message); } // invalid float
        try { testCode("5 + 5.565.5"); } catch (Exception e) { Console.WriteLine(e.Message); } // invalid float
        try { testCode("\"Hello world!..."); } catch (Exception e) { Console.WriteLine(e.Message); } // unterminated string
        try { testCode("5 $ 3"); } catch (Exception e) { Console.WriteLine(e.Message); } // $ is not a valid char
    }
}