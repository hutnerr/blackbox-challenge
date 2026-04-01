namespace Nodes
{
    public class Runtime
    {
        private Dictionary<string, IAstNode> variables; // use IAstNode since everything extends this

        public Runtime()
        {
            variables = new Dictionary<string, IAstNode>();
        }

        public void SetVariable(string name, IAstNode value)
        {
            variables[name] = value;
        }

        // only use after a check to HasVariable
        public object? GetVariable(string name)
        {
            if (variables.TryGetValue(name, out var value))
            {
                return value;
            }
            return null;
        }

        public bool HasVariable(string name)
        {
            return variables.ContainsKey(name);
        }
    }
}
