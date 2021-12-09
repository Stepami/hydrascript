using System;
using Interpreter.Lib.Semantic.Nodes;

namespace Interpreter.Lib.Semantic.Analysis
{
    public class SemanticAnalyzer
    {
        public Action<AbstractSyntaxTreeNode> CheckCallback { get; }
        
        public SemanticAnalyzer(Action<AbstractSyntaxTreeNode> checkCallback)
        {
            CheckCallback = checkCallback;
        }
    }
}