using FormulaBuilder.Core.Domain.Model.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain.Model
{
    public abstract class Executable
    {
        internal Executable() { }
        public abstract Number Execute();
    }
    public class Executable<T> : Executable
        where T:struct
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

        public override Number Execute()
        {
            Number result;

            var type = typeof(T);

            if (type == typeof(decimal))
                result = Formula.RootNode.Resolve(this as Executable<decimal>);
            else if (type == typeof(float))
                result = Formula.RootNode.Resolve(this as Executable<float>);
            else if (type == typeof(double))
                result = Formula.RootNode.Resolve(this as Executable<double>);
            else
                throw new InvalidOperationException($"Unsupported return type");

            return result;
        }

        internal void AddNestedFormulaResult(string key, T result)
        {
            if (_nestedFormulaResults.ContainsKey(key))
                throw new InvalidOperationException($"The result for key [{key}] has already been cached");

            _nestedFormulaResults.Add(key, result);
        }
    }
}
