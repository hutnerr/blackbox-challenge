using System;
using System.Collections.Generic;

namespace Nodes
{
    // for the operations that evaulate to a primitive
    // eg. everything besides whats in INodePrimitives
    public interface IAstNode
    {
        R Visit<R>(IVisitor<R> visitor);
        Tuple<int, int> getIndicies();
    }

    // for binary nodes
    public abstract class AbstractAst : IAstNode
    {
        private IAstNode Left;
        private IAstNode Right;

        private int startIndex;
        private int endIndex;

        public AbstractAst(IAstNode left, IAstNode right, int startIndex, int endIndex)
        {
            Left = left;
            Right = right;
            this.startIndex = startIndex;
            this.endIndex = endIndex;
        }

        // overload for passing in a tuple of indicies
        public AbstractAst(IAstNode left, IAstNode right, Tuple<int, int> indicies) : this(left, right, indicies.Item1, indicies.Item2)
        {
        }

        public abstract R Visit<R>(IVisitor<R> visitor);

        public IAstNode getLeft() { return Left; }
        public IAstNode getRight() { return Right; }

        public Tuple<int, int> getIndicies() { return Tuple.Create(startIndex, endIndex); }
    }

    // for unary nodes
    public abstract class AbstractAstUnary : IAstNode
    {
        private IAstNode Child;
        private int startIndex;
        private int endIndex;

        public AbstractAstUnary(IAstNode child, int startIndex, int endIndex)
        {
            Child = child;
            this.startIndex = startIndex;
            this.endIndex = endIndex;
        }

        // overload for passing in a tuple of indicies
        public AbstractAstUnary(IAstNode child, Tuple<int, int> indicies) : this(child, indicies.Item1, indicies.Item2)
        {
        }

        public abstract R Visit<R>(IVisitor<R> visitor);

        public IAstNode getChild() { return Child; }

        public Tuple<int, int> getIndicies() { return Tuple.Create(startIndex, endIndex); }
        public int getEndIndex() { return endIndex; }
    }

    // the 5 primitives: integer, float, boolean, string, and null
    public interface IPrimitiveNode : IAstNode { }
    public abstract class AbstractPrimitive<T> : IPrimitiveNode
    {
        public T Value;
        public int startIndex;
        public int endIndex;

        protected AbstractPrimitive(T value, int startIndex, int endIndex)
        {
            Value = value;
            this.startIndex = startIndex;
            this.endIndex = endIndex;
        }

        protected AbstractPrimitive(T value, Tuple<int, int> indicies) : this(value, indicies.Item1, indicies.Item2)
        {
        }

        public abstract R Visit<R>(IVisitor<R> visitor);

        public T getValue() { return Value; }

        public Tuple<int, int> getIndicies() { return Tuple.Create(startIndex, endIndex); }
    }
}
