namespace Tokens
{
    // wonder if enums are the best way to do this?
    public enum TokenType
    {
        // primitives
        INT,
        FLOAT,
        BOOL,
        STRING,
        NULL,

        // arithmetic
        ADD,
        SUBTRACTION_OR_NEGATION,
        MULTIPLY,
        DIVIDE,
        MODULO,
        EXPONENT,
        // NEGATION,

        // logical
        AND,
        OR,
        NOT,

        // bitwise
        BITAND,
        BITOR,
        BITXOR,
        BITNOT,
        BITLEFTSHIFT,
        BITRIGHTSHIFT,

        // relational
        EQUALS,
        NOTEQUALS,
        LESSTHAN,
        LESSTHANEQUALTO,
        GREATERTHAN,
        GREATERTHANEQUALTO,

        // casting
        CASTFLOAT,
        CASTINT,

        // statements
        PRINT,
        ASSIGNMENT,
        // BLOCK,

        // variables
        VARIABLE,

        // parentheses
        LEFTPAREN,
        RIGHTPAREN,
        LEFTBRACKET,
        RIGHTBRACKET,
    }

    public class Token
    {
        private TokenType type;
        private string text;
        private int startIndex;
        private int endIndex;

        public Token(TokenType type, string text, int startIndex, int endIndex)
        {
            this.type = type;
            this.text = text;
            this.startIndex = startIndex;
            this.endIndex = endIndex;
        }

        // getters
        public TokenType getType() { return type; }
        public string getText() { return text; }
        public int getStartIndex() { return startIndex; }
        public int getEndIndex() { return endIndex; }

        public int length()
        {
            return endIndex - startIndex;
        }

        public override string ToString()
        {
            return $"Token(Type: {type}, Text: '{text}', Start: {startIndex}, End: {endIndex})";
        }

        // FIXME: wonder what else could be useful here?
    }
}