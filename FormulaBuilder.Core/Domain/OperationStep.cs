using MiscUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain
{
    public abstract class OperationStep : FormulaStep
    {
        private static Dictionary<string, Func<int, OperationStep>> _registry = new Dictionary<string, Func<int, OperationStep>>()
        {
            {"+", (context) => new SumStep(context) }
        };

        private OperationStep(int contextId)
            : base(contextId)
        { }

        public static OperationStep Create(string key, int contextId)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            if (!_registry.ContainsKey(key))
                throw new InvalidOperationException($"key {key} not recognized. Valid keys are {string.Join(",", _registry.Keys)}");

            return _registry[key](contextId);
        }

        private class SumStep : OperationStep
        {
            internal SumStep(int contextId)
                : base(contextId)
            { }
            public override T Execute<T>(ExecutableFormula<T> formula)
            {
                T sum = Operator<T>.Zero;
                foreach (var operand in formula.GetOperands())
                {
                    sum = Operator.Add(sum, operand);
                }

                formula.ClearOperands();
                formula.SetOperand(ContextId, sum);

                return sum;
            }
        }

        private class SubtractionStep : OperationStep
        {
            internal SubtractionStep(int contextId)
                : base(contextId)
            { }
            public override T Execute<T>(ExecutableFormula<T> formula)
            {
                T difference = Operator<T>.Zero;
                var operands = formula.GetOperands().ToList();
     
                for(int i = 0; i < operands.Count; i++)
                {
                    if (i == 0)
                        difference = Operator.Add(difference, operands[i]);
                    else
                        difference = Operator.Subtract(difference, operands[i]);
                }

                formula.ClearOperands();
                formula.SetOperand(ContextId, difference);

                return difference;
            }
        }

        private class MultiplicationStep : OperationStep
        {
            internal MultiplicationStep(int contextId)
                : base(contextId)
            { }
            public override T Execute<T>(ExecutableFormula<T> formula)
            {
                T product = Operator<T>.Zero;
                var operands = formula.GetOperands().ToList();

                for (int i = 0; i < operands.Count; i++)
                {
                    if (i == 0)
                        product = Operator.Add(product, operands[i]);
                    else
                        product = Operator.Multiply(product, operands[i]);
                }

                formula.ClearOperands();
                formula.SetOperand(ContextId, product);

                return product;
            }
        }

        private class DivisionStep : OperationStep
        {
            internal DivisionStep(int contextId)
                : base(contextId)
            { }
            public override T Execute<T>(ExecutableFormula<T> formula)
            {
                T quotient = Operator<T>.Zero;
                var operands = formula.GetOperands().ToList();

                for (int i = 0; i < operands.Count; i++)
                {
                    if (i == 0)
                        quotient = Operator.Add(quotient, operands[i]);
                    else
                        quotient = Operator.Divide(quotient, operands[i]);
                }

                formula.ClearOperands();
                formula.SetOperand(ContextId, quotient);

                return quotient;
            }
        }
    }
}
