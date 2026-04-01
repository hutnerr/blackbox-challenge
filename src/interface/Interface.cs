using System;
using System.Data;
using System.Runtime.CompilerServices;
using Nodes;
using System.Text.RegularExpressions;
using Interpreter;
using Terminal.Gui;
using TAttr = Terminal.Gui.Attribute;

class TuiInterface
{
    // ================================
    //  UI ELEMENTS DECLARED UP TOP
    // ================================

    // Top table windows
    private Window? aColumn;
    private Window? bColumn;
    private Window? cColumn;
    private Window? actualColumn;
    private Window? expectedColumn;

    // Top table outputs
    private TextView? aColumnOutput;
    private TextView? bColumnOutput;
    private TextView? cColumnOutput;
    private TextView? actualColumnOutput;
    private TextView? expectedColumnOutput;

    // Middle input windows
    private Window? aEnterColumn;
    private Window? bEnterColumn;
    private Window? cEnterColumn;

    // Middle input text areas
    private TextView? aText;
    private TextView? bText;
    private TextView? cText;

    // Function window and text
    private Window? functionWin;
    private TextView? funcText;

    // Output window
    private Window? outputWin;

    // Output text area
    private TextView? outputText;

    // Instructions window and text
    private Window? instructionsWin;
    private TextView? instructionsText;

    // for running func
    private IAstNode? expectedAst;
    private string[]? ptypes;
    private int nparams;

    // ================================
    //  ADD METHODS CAN NOW USE THESE
    // ================================
    private void addToA(String text)
    {
        if (aColumnOutput != null)
        {
            aColumnOutput.Text += text + "\n";
        }
    }

    private void addToB(String text)
    {
        if (bColumnOutput != null)
        {
            bColumnOutput.Text += text + "\n";
        }
    }

    private void addToC(String text)
    {
        if (cColumnOutput != null)
        {
            cColumnOutput.Text += text + "\n";
        }
    }

    private void addToActual(String text)
    {
        if (actualColumnOutput != null)
        {
            actualColumnOutput.Text += text + "\n";
        }
    }

    private void addToExpected(String text)
    {
        if (expectedColumnOutput != null)
        {
            expectedColumnOutput.Text += text + "\n";
        }
    }

    private void addToOutput(String text)
    {
        if (outputText != null)
        {
            outputText.Text = text;
        }
    }

    private void runInput()
    {
        if (funcText == null || expectedAst == null || ptypes == null || aText == null || bText == null || cText == null)
        {
            addToOutput("uh oh");
            return;
        }

        // clear previous outputs
        actualColumnOutput!.Text = "";
        expectedColumnOutput!.Text = "";
        outputText!.Text = "";

        // get the parameters
        string[] aLines = aText.Text.ToString().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        string[] bLines = bText.Text.ToString().Split('\n', StringSplitOptions.RemoveEmptyEntries);
        string[] cLines = cText.Text.ToString().Split('\n', StringSplitOptions.RemoveEmptyEntries);

        string aInput = (nparams > 0) && aLines.Length > 0 ? $"a = {aLines[0]}" : "";
        string bInput = (nparams > 1) && bLines.Length > 0 ? $"b = {bLines[0]}" : "";
        string cInput = (nparams > 2) && cLines.Length > 0 ? $"c = {cLines[0]}" : "";

        if (nparams > 0 && aLines.Length == 0)
        {
            addToOutput("Parameter a is required");
            return;
        }

        if (nparams > 1 && bLines.Length == 0)
        {
            addToOutput("Parameter b is required");
            return;
        }

        if (nparams > 2 && cLines.Length == 0)
        {
            addToOutput("Parameter c is required");
            return;
        }

        // if a user doesnt provide a function, we still need to run the expected function
        // using the params. kinda patched this in thats why its messy lol
        bool evalUserFunction = true;
        if (funcText == null || funcText.Text == null || string.IsNullOrWhiteSpace(funcText.Text.ToString()))
        {
            evalUserFunction = false;
        }

        string[] results = {"", ""}; // user, expected

        if (evalUserFunction)
        {
            Lexer lexer = new(funcText.Text.ToString());
            lexer.tokenize();
            Parser parser = new(lexer.getTokens());
            IAstNode userAst = parser.parse();
            string userFunction = userAst.Visit(new Translator());

            // blocks are statements surrounded by brackets in our grammar
            String userBlockString = "{\n" +
                aInput + "\n" +
                bInput + "\n" +
                cInput + "\n" +
                userFunction + "\n" +
                "}";

            // evaluate user function
            Evaluator evaluator = new(new Runtime());
            try
            {
                Lexer userLexer = new(userBlockString);
                userLexer.tokenize();
                Parser userParser = new(userLexer.getTokens());
                IAstNode userBlockAst = userParser.parse();
                IPrimitiveNode userResultNode = userBlockAst.Visit(evaluator);
                String userResult;
                switch (userResultNode)
                {
                    case IntegerPrimitive i:
                        userResult = i.getValue().ToString();
                        break;
                    case FloatPrimitive f:
                        userResult = f.getValue().ToString();
                        break;
                    case BooleanPrimitive b:
                        userResult = b.getValue().ToString();
                        break;
                    case StringPrimitive s:
                        userResult = s.getValue();
                        break;
                    case NullPrimitive n:
                        userResult = "null";
                        break;
                    default:
                        userResult = "Unknown primitive type: " + userResultNode.GetType().Name;
                        break;
                }
                results[0] = userResult;
                addToActual($"{userResult}");
            }
            catch (Exception e)
            {
                addToOutput($"Error in user function: {e.Message}");
                return;
            }
        }

        // evaluate expected function
        String expectedBlockString = "{\n" +
            aInput + "\n" +
            bInput + "\n" +
            cInput + "\n" +
            expectedAst.Visit(new Translator()) + "\n" +
            "}";

        Evaluator expectedEvaluator = new(new Runtime());
        try
        {
            Lexer expectedLexer = new(expectedBlockString);
            expectedLexer.tokenize();
            Parser expectedParser = new(expectedLexer.getTokens());
            IAstNode expectedBlockAst = expectedParser.parse();
            IPrimitiveNode expectedResultNode = expectedBlockAst.Visit(expectedEvaluator);
            String expectedResult;
            switch (expectedResultNode)
            {
                case IntegerPrimitive i:
                    expectedResult = i.getValue().ToString();
                    break;
                case FloatPrimitive f:
                    expectedResult = f.getValue().ToString();
                    break;
                case BooleanPrimitive b:
                    expectedResult = b.getValue().ToString();
                    break;
                case StringPrimitive s:
                    expectedResult = s.getValue();
                    break;
                case NullPrimitive n:
                    expectedResult = "null";
                    break;
                default:
                    expectedResult = "Unknown primitive type: " + expectedResultNode.GetType().Name;
                    break;
            }
            results[1] = expectedResult;
            addToExpected($"{expectedResult}");
        }
        catch (Exception e)
        {
            addToOutput($"Error in expected function: {e.Message}");
            return;
        }

        // compare results
        if (evalUserFunction == false)
        {
            addToOutput("Use the expected result to determine what might go into the function!");
            return;
        }

        if (results[0] == results[1])
        {
            addToOutput("Great job!!! Actual matches Expected.");
        }
        else
        {
            addToOutput("Failure... Actual does not match Expected. :(");
        }
    }

    // ================================
    //  MAIN UI CONSTRUCTION
    // ================================
    public void TerminalApp(string[] lines)
    {
        Application.Init();
        try
        {
            Colors.Base = new ColorScheme()
            {
                Normal = new TAttr(Color.White, Color.Black),
                Focus = new TAttr(Color.Black, Color.Gray),
                HotNormal = new TAttr(Color.BrightYellow, Color.Black),
                HotFocus = new TAttr(Color.BrightYellow, Color.Gray),
                Disabled = new TAttr(Color.Gray, Color.Black)
            };

            Colors.Dialog = Colors.Base;
            Colors.Menu = Colors.Base;
            Colors.Error = Colors.Base;

            var top = Application.Top;
            Application.Driver.StopReportingMouseMoves();

            var oneThird = Dim.Percent(33);
            var y1 = Pos.Percent(33);
            var y2 = Pos.Percent(66);

            var x1 = Pos.Percent(10);
            var x2 = Pos.Percent(20);
            var x3 = Pos.Percent(30);
            var x4 = Pos.Percent(65);

            // ================================
            //  TABLE WINDOWS
            // ================================
            aColumn = new Window("A")
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(10),
                Height = oneThird,
            };

            aColumnOutput = new TextView()
            {
                ReadOnly = true,
                WordWrap = true,
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            aColumn?.Add(aColumnOutput);

            bColumn = new Window("B")
            {
                X = x1,
                Y = 0,
                Width = Dim.Percent(10),
                Height = oneThird
            };

            bColumnOutput = new TextView()
            {
                ReadOnly = true,
                WordWrap = true,
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            bColumn?.Add(bColumnOutput);

            cColumn = new Window("C")
            {
                X = x2,
                Y = 0,
                Width = Dim.Percent(10),
                Height = oneThird
            };

            cColumnOutput = new TextView()
            {
                ReadOnly = true,
                WordWrap = true,
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            cColumn?.Add(cColumnOutput);

            actualColumn = new Window("Actual")
            {
                X = x3,
                Y = 0,
                Width = Dim.Percent(35),
                Height = oneThird
            };

            actualColumnOutput = new TextView()
            {
                ReadOnly = true,
                WordWrap = true,
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            actualColumn?.Add(actualColumnOutput);

            expectedColumn = new Window("Expected")
            {
                X = x4,
                Y = 0,
                Width = Dim.Percent(35),
                Height = oneThird
            };

            expectedColumnOutput = new TextView()
            {
                ReadOnly = true,
                WordWrap = true,
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            expectedColumn?.Add(expectedColumnOutput);

            // ================================
            //  INPUT WINDOWS
            // ================================
            aEnterColumn = new Window("A")
            {
                X = Pos.Percent(70),
                Y = y1,
                Width = Dim.Percent(10),
                Height = oneThird
            };

            aText = new TextView() { X = 0, Y = 0, Width = Dim.Fill(), Height = Dim.Fill(), AllowsTab = false};
            aEnterColumn.Add(aText);

            bEnterColumn = new Window("B")
            {
                X = Pos.Percent(80),
                Y = y1,
                Width = Dim.Percent(10),
                Height = oneThird
            };

            bText = new TextView() { X = 0, Y = 0, Width = Dim.Fill(), Height = Dim.Fill(), AllowsTab = false };
            bEnterColumn.Add(bText);

            cEnterColumn = new Window("C")
            {
                X = Pos.Percent(90),
                Y = y1,
                Width = Dim.Percent(10),
                Height = oneThird
            };

            cText = new TextView() { X = 0, Y = 0, Width = Dim.Fill(), Height = Dim.Fill(), AllowsTab = false };
            cEnterColumn.Add(cText);

            // ================================
            //  FUNCTION WINDOW
            // ================================
            functionWin = new Window("Function")
            {
                X = 0,
                Y = y1,
                Width = Dim.Percent(70),
                Height = oneThird
            };

            funcText = new TextView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                AllowsTab = false
            };

            functionWin.Add(funcText);

            // ================================
            //  OUTPUT WINDOW
            // ================================
            outputWin = new Window("Output")
            {
                X = 0,
                Y = y2,
                Width = Dim.Percent(50),
                Height = Dim.Fill()
            };

            outputText = new TextView()
            {
                ReadOnly = true,
                WordWrap = true,   // enable wrapping
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            outputWin.Add(outputText);

            // ==================================
            // instructions
            // ================================
            instructionsWin = new Window("Instructions")
            {
                X = Pos.Percent(50),
                Y = y2,
                Width = Dim.Percent(50),
                Height = Dim.Fill()
            };

            instructionsText = new TextView()
            {
                ReadOnly = true,
                WordWrap = true,
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                Text = "1. the top row tells you output as well as what each variable expects\n" +
                       "2. put inputs into a, b, and c on the second row.\n" +
                       "3. use tab and shift tab to navigate\n" +
                       "4. ctrl + r will run with those inputs and display at the top\n" +
                       "5. once confident in what it might be, put it into the function box then run it\n\n" +
                       "grammar reference: \n" +
                       "- arithmetic: + - * / % \n" +
                       "- comparison: == != < > <= >= \n\n" +
                       "esc to quit at any time" + "\n"
            };

            instructionsWin.Add(instructionsText);

            // ================================
            //  GLOBAL KEY HANDLERS
            // ================================
            top.KeyDown += (args) =>
            {
                var k = args.KeyEvent;

                // esc to close
                if (k.Key == Key.Esc)
                {
                    Application.RequestStop();
                    args.Handled = true;
                    return;
                }

                // ctrl + r to run
                if (k.Key == (Key.R | Key.CtrlMask))
                {
                    runInput();
                    args.Handled = true;
                    return;
                }
            };

            // ================================
            //  ADD WINDOWS TO ROOT
            // ================================
            top.Add(
                aColumn, bColumn, cColumn,
                actualColumn, expectedColumn,
                functionWin,
                aEnterColumn, bEnterColumn, cEnterColumn,
                outputWin, instructionsWin
            );

            // ================================
            // parse the lines from the file
            // ================================

            // int int
            // sum = a + b
            // sum / 2

            // int
            // a + 5

            // file format is
            // first line: paramter types, also the number of parameters
            // rest of lines should be used to build the ast

            ptypes = lines[0].Split(' ');
            nparams = ptypes.Length;

            for (int i = 0; i < nparams; i++)
            {
                switch (i)
                {
                    case 0:
                        addToA(ptypes[i]);
                        break;
                    case 1:
                        addToB(ptypes[i]);
                        break;
                    case 2:
                        addToC(ptypes[i]);
                        break;
                }
            }

            // use the rest of the lines to build the test case
            Lexer lexer = new(string.Join("\n", lines, 1, lines.Length - 1));
            lexer.tokenize();
            Parser parser = new(lexer.getTokens());
            expectedAst = parser.parse();

            Application.Run();
        }
        finally
        {
            Application.Shutdown();
        }
    }
}
