using System.Resources;
using System.Security.Cryptography;
using System.Security.Permissions;
using Nodes;
using Tokens;

namespace Interpreter
{
    public class Parser
    {
        private List<Tokens.Token> tokens;
        private int currentIndex;

        public Parser(List<Tokens.Token> tokens)
        {
            this.tokens = tokens;
            this.currentIndex = 0;
        }

        public List<Tokens.Token> getTokens() { return tokens; }

        // reset the parser to the beginning of the token list
        private void reset()
        {
            this.currentIndex = 0;
        }

        // move to the next token without capturing it
        private void advance()
        {
            this.currentIndex++;
            if (isAtEnd())
            {
                this.currentIndex = tokens.Count; // stay at end
            }
        }

        // check if the current token matches t
        private bool has(Tokens.TokenType t)
        {
            if (isAtEnd()) return false;
            return tokens[currentIndex].getType() == t;
        }

        private bool isAtEnd()
        {
            return currentIndex >= tokens.Count;
        }


        public IAstNode parse()
        {
            List<IAstNode> program = programNonTerminal();
            return new ProgramNode(program, program[0].getIndicies().Item1, program[program.Count - 1].getIndicies().Item2);
        }

        // grammar methods
        private List<IAstNode> programNonTerminal()
        {
            List<IAstNode> expressions = new List<IAstNode>();
            while (!isAtEnd())
            {
                IAstNode expr = expresionNonTerminal();
                expressions.Add(expr);
            }
            return expressions;
        }

        private IAstNode expresionNonTerminal()
        {
            if (has(TokenType.PRINT))
            {
                int startIndex = tokens[currentIndex].getStartIndex();
                advance();
                IAstNode child = level0NonTerminal();
                return new PrintNode(child, startIndex, child.getIndicies().Item2);
            }
            else if (has(TokenType.VARIABLE))
            {
                string varName = tokens[currentIndex].getText();
                int startIndex = tokens[currentIndex].getStartIndex();
                int endIndex = tokens[currentIndex].getEndIndex();
                advance();
                if (has(TokenType.ASSIGNMENT))
                {
                    advance();
                    IAstNode right = level0NonTerminal();
                    return new AssignmentNode(new VariableNode(varName, startIndex, endIndex), right, startIndex, right.getIndicies().Item2);
                }
                else
                {
                    currentIndex--;
                    return level0NonTerminal();
                }
            }
            else if (has(TokenType.LEFTBRACKET))
            {
                int startIndex = tokens[currentIndex].getStartIndex();
                advance();
                List<IAstNode> statements = new List<IAstNode>();
                while (!has(TokenType.RIGHTBRACKET) && !isAtEnd())
                {
                    IAstNode statement = expresionNonTerminal();
                    statements.Add(statement);
                }
                if (has(TokenType.RIGHTBRACKET))
                {
                    int endIndex = tokens[currentIndex].getEndIndex();
                    advance();
                    return new BlockNode(statements, startIndex, endIndex);
                }
                else
                {
                    throw new Exception("Expected closing bracket for block");
                }
            }
            else
            {
                return level0NonTerminal();
            }
        }

        private IAstNode level0NonTerminal()
        {
            IAstNode left = level1NonTerminal();

            while (has(TokenType.OR))
            {
                advance();
                IAstNode right = level1NonTerminal();
                left = new OrNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
            }

            return left;
        }

        private IAstNode level1NonTerminal()
        {
            IAstNode left = level2NonTerminal();

            while (has(TokenType.AND))
            {
                advance();
                IAstNode right = level2NonTerminal();
                left = new AndNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
            }

            return left;
        }

        private IAstNode level2NonTerminal()
        {
            IAstNode left = level3NonTerminal();

            while (has(TokenType.BITOR))
            {
                advance();
                IAstNode right = level3NonTerminal();
                left = new BitOrNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
            }

            return left;
        }

        private IAstNode level3NonTerminal()
        {
            IAstNode left = level4NonTerminal();

            while (has(TokenType.BITXOR))
            {
                advance();
                IAstNode right = level4NonTerminal();
                left = new BitXorNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
            }

            return left;
        }

        private IAstNode level4NonTerminal()
        {
            IAstNode left = level5NonTerminal();

            while (has(TokenType.BITAND))
            {
                advance();
                IAstNode right = level5NonTerminal();
                left = new BitAndNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
            }

            return left;
        }

        private IAstNode level5NonTerminal()
        {
            IAstNode left = level6NonTerminal();

            while (has(TokenType.EQUALS) || has(TokenType.NOTEQUALS))
            {
                if (has(TokenType.EQUALS))
                {
                    advance();
                    IAstNode right = level6NonTerminal();
                    left = new EqualsNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
                }
                else if (has(TokenType.NOTEQUALS))
                {
                    advance();
                    IAstNode right = level6NonTerminal();
                    left = new NotEqualsNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
                }
            }

            return left;
        }

        private IAstNode level6NonTerminal()
        {
            IAstNode left = level7NonTerminal();

            while (has(TokenType.LESSTHAN) || has(TokenType.LESSTHANEQUALTO) || has(TokenType.GREATERTHAN) || has(TokenType.GREATERTHANEQUALTO))
            {
                if (has(TokenType.LESSTHAN))
                {
                    advance();
                    IAstNode right = level7NonTerminal();
                    left = new LessThanNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
                }
                else if (has(TokenType.LESSTHANEQUALTO))
                {
                    advance();
                    IAstNode right = level7NonTerminal();
                    left = new LessThanEqualToNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
                }
                else if (has(TokenType.GREATERTHAN))
                {
                    advance();
                    IAstNode right = level7NonTerminal();
                    left = new
                    GreaterThanNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
                }
                else if (has(TokenType.GREATERTHANEQUALTO))
                {
                    advance();
                    IAstNode right = level7NonTerminal();
                    left = new GreaterThanEqualToNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
                }
            }

            return left;
        }

        private IAstNode level7NonTerminal()
        {
            IAstNode left = level8NonTerminal();

            while (has(TokenType.BITLEFTSHIFT) || has(TokenType.BITRIGHTSHIFT))
            {
                if (has(TokenType.BITLEFTSHIFT))
                {
                    advance();
                    IAstNode right = level8NonTerminal();
                    left = new BitLeftShiftNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
                }
                else if (has(TokenType.BITRIGHTSHIFT))
                {
                    advance();
                    IAstNode right = level8NonTerminal();
                    left = new BitRightShiftNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
                }
            }

            return left;
        }

        private IAstNode level8NonTerminal()
        {
            IAstNode left = level9NonTerminal();

            while (has(TokenType.ADD) || has(TokenType.SUBTRACTION_OR_NEGATION))
            {
                if (has(TokenType.ADD))
                {
                    advance();
                    IAstNode right = level9NonTerminal();
                    left = new AddNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
                }
                else if (has(TokenType.SUBTRACTION_OR_NEGATION))
                {
                    advance();
                    IAstNode right = level9NonTerminal();
                    left = new SubtractNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
                }
            }

            return left;
        }

        private IAstNode level9NonTerminal()
        {
            IAstNode left = level10NonTerminal();

            while (has(TokenType.MULTIPLY) || has(TokenType.DIVIDE) || has(TokenType.MODULO))
            {
                if (has(TokenType.MULTIPLY))
                {
                    advance();
                    IAstNode right = level10NonTerminal();
                    left = new MultiplyNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
                }
                else if (has(TokenType.DIVIDE))
                {
                    advance();
                    IAstNode right = level10NonTerminal();
                    left = new DivideNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
                }
                else if (has(TokenType.MODULO))
                {
                    advance();
                    IAstNode right = level10NonTerminal();
                    left = new ModuloNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
                }
            }

            return left;
        }

        private IAstNode level10NonTerminal()
        {
            if (has(TokenType.BITNOT))
                {
                    advance();
                    IAstNode right = level10NonTerminal();
                    return new BitNotNode(right, tokens[currentIndex - 1].getStartIndex(), right.getIndicies().Item2);
                }
                else if (has(TokenType.SUBTRACTION_OR_NEGATION))
                {
                    advance();
                    IAstNode right = level10NonTerminal();
                    return new NegationNode(right, tokens[currentIndex - 1].getStartIndex(), right.getIndicies().Item2);
                }
                else if (has(TokenType.NOT))
                {
                    advance();
                    IAstNode right = level10NonTerminal();
                    return new NotNode(right, tokens[currentIndex - 1].getStartIndex(), right.getIndicies().Item2);
                }

            return level11NonTerminal();
        }

        private IAstNode level11NonTerminal()
        {
            IAstNode left = level12NonTerminal();
            if (has(TokenType.EXPONENT))
            {
                advance();
                IAstNode right = level11NonTerminal();
                left = new ExponentNode(left, right, left.getIndicies().Item1, right.getIndicies().Item2);
            }

            return left;
        }

        private IAstNode level12NonTerminal()
        {
            if (has(TokenType.CASTINT))
                {
                    advance();
                    IAstNode right = level0NonTerminal();
                    return new FloatToInt(right, tokens[currentIndex - 1].getStartIndex(), right.getIndicies().Item2);
                }
                else if (has(TokenType.CASTFLOAT))
                {
                    advance();
                    IAstNode right = level0NonTerminal();
                    return new IntToFloat(right, tokens[currentIndex - 1].getStartIndex(), right.getIndicies().Item2);
                }

            return level13NonTerminal();
        }

        private IAstNode level13NonTerminal()
        {
            if (has(TokenType.LEFTPAREN))
            {
                advance();
                IAstNode expr = level0NonTerminal();
                if (has(TokenType.RIGHTPAREN))
                {
                    advance();
                    return expr;
                }
                else
                {
                    throw new Exception("Expected closing parenthesis, opened at index " + tokens[currentIndex - 1].getStartIndex());
                }
            }
            else
            {
                return levelNNonTerminal();
            }
        }

        private IAstNode levelNNonTerminal()
        {
            if (has(TokenType.INT))
            {
                int result = int.Parse(tokens[currentIndex].getText());
                int startIndex = tokens[currentIndex].getStartIndex();
                int endIndex = tokens[currentIndex].getEndIndex();
                advance();
                return new IntegerPrimitive(result, startIndex, endIndex);
            }
            else if (has(TokenType.FLOAT))
            {
                float result = float.Parse(tokens[currentIndex].getText());
                int startIndex = tokens[currentIndex].getStartIndex();
                int endIndex = tokens[currentIndex].getEndIndex();
                advance();
                return new FloatPrimitive(result, startIndex, endIndex);
            }
            else if (has(TokenType.BOOL))
            {
                bool result = bool.Parse(tokens[currentIndex].getText());
                int startIndex = tokens[currentIndex].getStartIndex();
                int endIndex = tokens[currentIndex].getEndIndex();
                advance();
                return new BooleanPrimitive(result, startIndex, endIndex);
            }
            else if (has(TokenType.STRING))
            {
                string result = tokens[currentIndex].getText();
                int startIndex = tokens[currentIndex].getStartIndex();
                int endIndex = tokens[currentIndex].getEndIndex();
                advance();
                return new StringPrimitive(result, startIndex, endIndex);
            }
            else if (has(TokenType.NULL))
            {
                int startIndex = tokens[currentIndex].getStartIndex();
                int endIndex = tokens[currentIndex].getEndIndex();
                advance();
                return new NullPrimitive(startIndex, endIndex);
            }
            else if (has(TokenType.VARIABLE))
            {
                string varName = tokens[currentIndex].getText();
                int startIndex = tokens[currentIndex].getStartIndex();
                int endIndex = tokens[currentIndex].getEndIndex();
                advance();
                return new VariableNode(varName, startIndex, endIndex);
            }
            else
            {
                throw new Exception($"Unexpected token: {tokens[currentIndex].getText()} at index {tokens[currentIndex].getStartIndex()}");
            }
        }
    }
}