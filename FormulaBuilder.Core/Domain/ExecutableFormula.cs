using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain
{
    public class ExecutableFormula<T>
    {
        internal Stack<FormulaStep> FormulaSteps { get; }
        internal HashSet<string> RequiredTokens { get; }

        internal ExecutableFormula(Stack<FormulaStep> formulaSteps, HashSet<string> requiredTokens)
        {
            if (formulaSteps == null)
                throw new ArgumentNullException(nameof(formulaSteps));
            if (requiredTokens == null)
                throw new ArgumentNullException(nameof(requiredTokens));

            FormulaSteps = formulaSteps;
            RequiredTokens = requiredTokens;
            _operands = new Dictionary<int, List<T>>();
        }

        private T runningTotal;

        private int _maxContext => _operands.Any() ? _operands.Keys.Max() : 0;
        private Dictionary<int,List<T>> _operands = new Dictionary<int, List<T>>();
        internal Dictionary<string, T> _tokens;

        public void ClearOperands()
        {
            _operands.Remove(_maxContext);
        }

        public IEnumerable<T> GetOperands()
        {
            return _operands.ContainsKey(_maxContext) ?
                _operands[_maxContext] :
                new List<T>();               
        }

        public void SetOperand(int contextId, T value)
        {
            if (_operands.ContainsKey(contextId))
                _operands[contextId].Add(value);
            else
                _operands.Add(contextId, new List<T>() { value });
        }

        public T Execute(Dictionary<string, T> tokens)
        {
            if (RequiredTokens.Except(tokens.Keys).Count() > 0)
                throw new InvalidOperationException("missing tokens");

            _tokens = tokens;

            while(FormulaSteps.Count > 0)
            {
                var step = FormulaSteps.Pop();
                runningTotal = step.Execute<T>(this);
            }

            return runningTotal;
        }
    }
}
