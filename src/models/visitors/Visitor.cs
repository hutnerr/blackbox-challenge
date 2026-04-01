using System.ComponentModel;

namespace Nodes
{
    public interface IVisitor<R>
    {
        // primitives
        R Visit(IntegerPrimitive node);
        R Visit(FloatPrimitive node);
        R Visit(BooleanPrimitive node);
        R Visit(StringPrimitive node);
        R Visit(NullPrimitive node);

        // arithmetic
        R Visit(AddNode node);
        R Visit(SubtractNode node);
        R Visit(MultiplyNode node);
        R Visit(DivideNode node);
        R Visit(ModuloNode node);
        R Visit(ExponentNode node);
        R Visit(NegationNode node); // unary

        // logical
        R Visit(AndNode node);
        R Visit(OrNode node);
        R Visit(NotNode node); // unary

        // bitwise
        R Visit(BitAndNode node);
        R Visit(BitOrNode node);
        R Visit(BitXorNode node);
        R Visit(BitNotNode node); // unary
        R Visit(BitLeftShiftNode node);
        R Visit(BitRightShiftNode node);

        // relational
        R Visit(EqualsNode node);
        R Visit(NotEqualsNode node);
        R Visit(LessThanNode node);
        R Visit(LessThanEqualToNode node);
        R Visit(GreaterThanNode node);
        R Visit(GreaterThanEqualToNode node);

        // casting
        R Visit(FloatToInt node); // unary
        R Visit(IntToFloat node); // unary

        // statements
        R Visit(PrintNode node);
        R Visit(AssignmentNode node);
        R Visit(BlockNode node);
        R Visit(ProgramNode node);

        // variables
        R Visit(VariableNode node);
    }
}
