using FormulaBuilder.Core.Domain.Model.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain.Model
{
    public class Executable<T>
    {
        public Formula Formula { get; }
        public IReadOnlyDictionary<string, Parameter> Parameters { get; }

        private readonly Dictionary<string, T> _nestedFormulaResults = new Dictionary<string, T>();

        public IReadOnlyDictionary<string, T> NestedFormulaResults { get { return _nestedFormulaResults; } }

        internal Executable(
           Formula formula,
           IReadOnlyDictionary<string, Parameter> parameters)
        {
            Formula = formula;
            Parameters = parameters;
        }

        public T Execute()
        {
            return Formula.RootNode.Resolve<T>(this);
        }

        internal void AddNestedFormulaResult(string key, T result)
        {
            if (_nestedFormulaResults.ContainsKey(key))
                throw new InvalidOperationException($"The result for key [{key}] has already been cached");

            _nestedFormulaResults.Add(key, result);
        }
    }
}
