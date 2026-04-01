using System.Resources;
using System.Security.Cryptography;
using System.Security.Permissions;
using Tokens;

namespace Interpreter
{
    public class Lexer
    {
        private string source;
        private int currentIndex;
        private int tokenStartIndex;
        private string tokenSoFar;
        private List<Tokens.Token> tokens;

        public Lexer(string source)
        {
            this.source = source;
            this.tokens = new List<Tokens.Token>();
            this.tokenSoFar = "";
            reset();
        }

        public string getSource() { return source; }
        public List<Tokens.Token> getTokens() { return tokens; }

        // reset the lexer to the beginning of the source code
        private void reset()
        {
            this.currentIndex = 0;
            this.tokenStartIndex = 0;
            this.tokenSoFar = "";
            this.tokens.Clear();
        }

        // move to the next char without capturing it
        private void skip()
        {
            this.currentIndex++;
        }

        private char prev()
        {
            if (currentIndex > 0 && currentIndex <= source.Length)
            {
                return source[currentIndex - 1];
            }
            throw new Exception($"LEXING ERROR: No previous character at index {currentIndex}"); // changed
        }

        // skip the current char and abandon the token we've been building
        private void abandon()
        {
            skip();
            tokenStartIndex = currentIndex;
            tokenSoFar = "";
        }

        // check if the current char matches c
        private bool has(char c)
        {
            return currentIndex < source.Length && source[currentIndex] == c;
        }

        private bool hasChar()
        {
            return currentIndex < source.Length && Char.IsLetter(source[currentIndex]);
        }

        private bool hasDigit()
        {
            return currentIndex < source.Length && Char.IsDigit(source[currentIndex]);
        }

        private bool hasWhitespace()
        {
            return currentIndex < source.Length && Char.IsWhiteSpace(source[currentIndex]);
        }

        // this keeps track of the tokens we've eaten
        // these chars will be part of the next token
        private void capture()
        {
            if (currentIndex < source.Length)
            {
                tokenSoFar += source[currentIndex];
                currentIndex++;
            }
        }

        // if this is called, we've found a token
        private void emitToken(Tokens.TokenType type)
        {
            Token token = new Token(type, tokenSoFar, tokenStartIndex, currentIndex - 1);
            tokens.Add(token);
            tokenStartIndex = currentIndex; // reset index for next token
            tokenSoFar = ""; // reset text as well
        }

        // maybe have this return the list of tokens?
        // right now they have to call getTokens() afterwards
        // this method is going to get crazy big
        public void tokenize()
        {
            // https://dear-computer.twodee.org/programming-language/lexing.html
            // couple examples here

            // BLOCKS are represented by { } in our grammar
            // so there is no BLOCK token, just LEFTBRACKET and RIGHTBRACKET tokens
            // inside of these is the block itself

            // FUNCTION CALLS (ie print ( expr )  or float ( expr ) )
            // get sent out when print or float is seen, etc.
            // the parens will be sent out as LEFTPAREN and RIGHTPAREN tokens
            // inside the parens is the expression to be evaluated

            // Wasn't too sure on the best way to handle negation vs subtraction
            // so I just made one token for both and the parser will have to decide based on context
            // it should be able to tell based on what comes before/after it

            // FIXME: what type of errors should I handle here?
            // some are clear like unterminated strings or invalid characters
            // but what about missing closing parens, etc.? parser problem?

            reset(); // start from the beginning

            while (currentIndex < source.Length)
            {
                if (hasWhitespace()) // skip whitespace
                {
                    abandon();
                }
                // ===== ARITHMETIC ===== 
                else if (has('+'))
                {
                    capture();
                    emitToken(Tokens.TokenType.ADD); // ADDITION
                }
                else if (has('-'))
                {
                    // parser will have to decide based on context whether this is a subtraction or negation
                    capture();
                    emitToken(Tokens.TokenType.SUBTRACTION_OR_NEGATION); // SUBTRACTION OR NEGATION
                }
                else if (has('*'))
                {
                    capture();
                    if (has('*'))
                    {
                        capture();
                        emitToken(Tokens.TokenType.EXPONENT); // EXPONENTIATION
                    }
                    else
                        emitToken(Tokens.TokenType.MULTIPLY); // MULTIPLICATION
                }
                else if (has('/'))
                {
                    capture();
                    emitToken(Tokens.TokenType.DIVIDE); // DIVISION
                }
                else if (has('%'))
                {
                    capture();
                    emitToken(Tokens.TokenType.MODULO); // MODULO
                }
                // ===== LOGICAL =====
                else if (has('&'))
                {
                    capture();
                    if (has('&'))
                    {
                        capture();
                        emitToken(Tokens.TokenType.AND); // AND
                    }
                    else
                    {
                        emitToken(Tokens.TokenType.BITAND); // BITWISE AND
                    }
                }
                else if (has('|'))
                {
                    capture();
                    if (has('|'))
                    {
                        capture();
                        emitToken(Tokens.TokenType.OR); // OR
                    }
                    else
                    {
                        emitToken(Tokens.TokenType.BITOR); // BITWISE OR
                    }
                }
                // ===== BITWISE =====
                else if (has('^'))
                {
                    capture();
                    emitToken(Tokens.TokenType.BITXOR); // BITWISE XOR
                }
                else if (has('~'))
                {
                    capture();
                    emitToken(Tokens.TokenType.BITNOT); // BITWISE NOT
                }
                // ===== RELATIONAL =====
                else if (has('!'))
                {
                    capture();
                    if (has('='))
                    {
                        capture();
                        emitToken(Tokens.TokenType.NOTEQUALS); // NOTEQUALS
                    }
                    else
                    {
                        emitToken(Tokens.TokenType.NOT); // LOGICAL NOT
                    }
                }
                else if (has('<'))
                {
                    capture();
                    if (has('='))
                    {
                        capture();
                        emitToken(Tokens.TokenType.LESSTHANEQUALTO); // LESSTHANEQUALTO
                    }
                    else if (has('<'))
                    {
                        capture();
                        emitToken(Tokens.TokenType.BITLEFTSHIFT); // BITWISE LEFT SHIFT
                    }
                    else
                    {
                        emitToken(Tokens.TokenType.LESSTHAN); // LESSTHAN
                    }
                }
                else if (has('>'))
                {
                    capture();
                    if (has('='))
                    {
                        capture();
                        emitToken(Tokens.TokenType.GREATERTHANEQUALTO); // GREATERTHANEQUALTO
                    }
                    else if (has('>'))
                    {
                        capture();
                        emitToken(Tokens.TokenType.BITRIGHTSHIFT); // BITWISE RIGHT SHIFT
                    }
                    else
                    {
                        emitToken(Tokens.TokenType.GREATERTHAN); // GREATERTHAN
                    }
                }
                else if (has('='))
                {
                    capture();
                    if (has('='))
                    {
                        capture();
                        emitToken(Tokens.TokenType.EQUALS);
                    }
                    else
                    {
                        emitToken(Tokens.TokenType.ASSIGNMENT);
                    }
                }
                // capture whole numbers. rn its only integers
                else if (hasDigit())
                {
                    bool seenDec = false;
                    while (hasDigit()) // get initial digits (or int)
                    {
                        capture();
                    }

                    if (has('.')) // once we've got our initial digits, if next is a decimal, we got a float
                    {
                        seenDec = true;
                        capture();

                        if (!hasDigit()) // must have at least one digit after decimal
                        {
                            throw new Exception($"LEXING ERROR: Expected digit after decimal point at index {currentIndex}: {tokenSoFar + source[currentIndex]}");
                        }

                        while (hasDigit()) // get digits after decimal
                        {
                            capture();
                        }

                        if (has('.')) // if we see another decimal, this is an error
                        {
                            throw new Exception($"LEXING ERROR: Overeager float error '{source[currentIndex]}' at index {currentIndex}: {tokenSoFar + source[currentIndex]}");
                        }
                    }
                    emitToken(seenDec ? Tokens.TokenType.FLOAT : Tokens.TokenType.INT);
                }
                // float without leading digit like .5
                else if (has('.'))
                {
                    capture(); // capture the decimal point

                    if (!hasDigit()) // must have at least one digit after decimal
                    {
                        throw new Exception($"LEXING ERROR: Expected digit after decimal point at index {currentIndex}: {tokenSoFar + source[currentIndex]}");
                    }

                    while (hasDigit()) // get digits after decimal
                    {
                        capture();
                    }
                    emitToken(Tokens.TokenType.FLOAT); // FLOAT
                }
                // capture a string
                else if (has('"'))
                {
                    // FIXME: do we handle escape chars?

                    capture(); // capture opening quote

                    while (currentIndex < source.Length && !has('"'))
                    {
                        capture();
                    }

                    if (has('"')) // make sure we have a closing quote
                    {
                        capture(); // capture closing quote
                        emitToken(Tokens.TokenType.STRING); // STRING
                    }
                    else
                    {
                        throw new Exception($"LEXING ERROR: Unclosed string, starting at index {tokenStartIndex}: {tokenSoFar}");
                    }
                }
                // capture a boolean or null
                // t for true, f for false, n for null
                // also capture print, and casting keywords
                // p for print, f for float, i for int
                else if (has('t') || has('f') || has('n') || has('p') || has('i'))
                {
                    while (hasChar()) // get the whole word
                    {
                        capture();
                    }

                    if (tokenSoFar == "true" || tokenSoFar == "false")
                    {
                        emitToken(Tokens.TokenType.BOOL); // BOOL
                    }
                    else if (tokenSoFar == "null")
                    {
                        emitToken(Tokens.TokenType.NULL); // NULL
                    }
                    // the below calls will contain LPAREN and RPAREN tokens as well
                    // when we get to parsing, we will handle them appropriately
                    else if (tokenSoFar == "float")
                    {
                        emitToken(Tokens.TokenType.CASTFLOAT); // CASTFLOAT (FLOAT TO INT)
                    }
                    else if (tokenSoFar == "int")
                    {
                        emitToken(Tokens.TokenType.CASTINT); // CASTINT (INT TO FLOAT)
                    }
                    else if (tokenSoFar == "print")
                    {
                        emitToken(Tokens.TokenType.PRINT); // PRINT
                    }
                    else
                    {
                        emitToken(Tokens.TokenType.VARIABLE); // if its not one of those, its a VARIABLE
                    }
                }
                else if (hasChar()) // capture a variable
                {
                    while (hasChar() || hasDigit() || has('_')) // get the whole word
                    {
                        capture();
                    }
                    emitToken(Tokens.TokenType.VARIABLE); // VARIABLE
                }
                else if (has('{'))
                {
                    capture();
                    emitToken(Tokens.TokenType.LEFTBRACKET); // BLOCK START
                }
                else if (has('}'))
                {
                    capture();
                    emitToken(Tokens.TokenType.RIGHTBRACKET); // BLOCK END
                }
                else if (has('('))
                {
                    capture();
                    emitToken(Tokens.TokenType.LEFTPAREN); // LEFT PAREN
                }
                else if (has(')'))
                {
                    capture();
                    emitToken(Tokens.TokenType.RIGHTPAREN); // RIGHT PAREN
                }
                else
                {
                    throw new Exception($"LEXING ERROR: Unexpected character '{source[currentIndex]}' at index {currentIndex}: {tokenSoFar + source[currentIndex]}");
                }
            }
        }
    }
}