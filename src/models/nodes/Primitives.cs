using Nodes;

public class IntegerPrimitive : AbstractPrimitive<int>
{
    public IntegerPrimitive(int value, int startIndex, int endIndex) : base(value, startIndex, endIndex) { }
    public IntegerPrimitive(int value, Tuple<int, int> indicies) : base(value, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class FloatPrimitive : AbstractPrimitive<double>
{
    public FloatPrimitive(double value, int startIndex, int endIndex) : base(value, startIndex, endIndex) { }
    public FloatPrimitive(double value, Tuple<int, int> indicies) : base(value, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class BooleanPrimitive : AbstractPrimitive<bool>
{
    public BooleanPrimitive(bool value, int startIndex, int endIndex) : base(value, startIndex, endIndex) { }
    public BooleanPrimitive(bool value, Tuple<int, int> indicies) : base(value, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}


public class StringPrimitive : AbstractPrimitive<string>
{
    public StringPrimitive(string value, int startIndex, int endIndex) : base(value, startIndex, endIndex) { }
    public StringPrimitive(string value, Tuple<int, int> indicies) : base(value, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}

public class NullPrimitive : AbstractPrimitive<object>
{
    public NullPrimitive(int startIndex, int endIndex) : base(null!, startIndex, endIndex) { }
    public NullPrimitive(Tuple<int, int> indicies) : base(null!, indicies) { }
    public override R Visit<R>(IVisitor<R> visitor) { return visitor.Visit(this); }
}
