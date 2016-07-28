using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain
{
    public class TokenStep : FormulaStep
    {
        public string Value { get; }
        public TokenStep(string value, int contextId)
            : base(contextId)
        {
            Value = value;
        }

        public override T Execute<T>(ExecutableFormula<T> formula)
        {
            var substitutedValue = formula._tokens[Value];
            formula.SetOperand(ContextId, substitutedValue);
            return substitutedValue;
        }
    }
}
