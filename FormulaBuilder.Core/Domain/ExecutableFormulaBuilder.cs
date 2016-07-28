using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;

namespace FormulaBuilder.Core.Domain
{
    public class ExecutableFormulaBuilder
    {
        private readonly Stack<FormulaStep> _formulaSteps;
        private readonly HashSet<string> _requiredTokens;
        private int _currentContext;
        private ExecutableFormulaBuilder()
        {
            _formulaSteps = new Stack<FormulaStep>();
            _requiredTokens = new HashSet<string>();
        }

        public static ExecutableFormulaBuilder Initialize()
        {
            return new ExecutableFormulaBuilder();
        }

        public ExecutableFormulaBuilder EnterContext()
        {
            _currentContext++;
            return this;
        }

        public ExecutableFormulaBuilder ExitContext()
        {
            _currentContext--;
            return this;
        }

        public ExecutableFormulaBuilder WithStep(Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));


            if (string.IsNullOrWhiteSpace(node.Type))
                throw new InvalidOperationException();

            var nodeType = node.Type;
            FormulaStep step;

            switch(nodeType)
            {
                case "operator":
                    step = OperationStep.Create(node.Value, _currentContext);
                    break;
                case "token":
                    step = new TokenStep(node.Value, _currentContext);
                    _requiredTokens.Add(node.Value);
                    break;
                default:
                    throw new InvalidOperationException($"node type {nodeType} is not recognized.");
            }

            _formulaSteps.Push(step);
            return this;
        }

        public ExecutableFormula<T> Build<T>()
        {
            if (_formulaSteps.Count == 0)
                throw new InvalidOperationException("Formula must have at least one step");
            if (_requiredTokens.Count == 0)
                throw new InvalidOperationException("Formula must have at least one token");

            return new ExecutableFormula<T>(_formulaSteps, _requiredTokens);
        }
    }
}
