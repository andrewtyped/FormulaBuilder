using FormulaBuilder.Core.Domain.Model.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain.Model
{
    public interface IParameterSetter<T>
    {
        IParameterSetter<T> WithParameter(Parameter parameter);
        IExecutable<T> AndNoMoreParameters();
    }

    public interface IExecutable<T>
    {
        T Execute();
        IParameterSetter<T> Reset();
    }

    public class ExecutableFormula<T> : Formula, IParameterSetter<T>, IExecutable<T>
    {
        private readonly Dictionary<string, Parameter> _parameters = new Dictionary<string, Parameter>();
        public IReadOnlyDictionary<string, Parameter> Parameters { get { return _parameters; } }

        private readonly Dictionary<string, T> _nestedFormulaResults = new Dictionary<string, T>();

        public IReadOnlyDictionary<string, T> NestedFormulaResults { get { return _nestedFormulaResults; } }

        internal ExecutableFormula(
           int id,
           string name,
           Node rootNode,
           IEnumerable<Formula> nestedFormulas)
            : base(id, name, rootNode, nestedFormulas)
        {
        }

        public IParameterSetter<T> WithParameter(Parameter parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (RequiredParameters.Contains(parameter.Name) == false)
                throw new InvalidOperationException($"Formula doesn't require a parameter named {parameter.Name}. Expected parameter names are {string.Join(", " + Environment.NewLine, RequiredParameters)}");

            _parameters.Add(parameter.Name, parameter);

            return this;
        }

        public IExecutable<T> AndNoMoreParameters()
        {
            if (HasMissingParameters())
                throw new InvalidOperationException($"The following required parameters are missing: {string.Join(", " + Environment.NewLine, GetMissingParameters())}");

            return this;
        }

        public IParameterSetter<T> Reset()
        {
            _parameters.Clear();
            _nestedFormulaResults.Clear();
            return this;
        }

        public T Execute()
        {
            if (HasMissingParameters())
                throw new InvalidOperationException($"The following required parameters are missing: {string.Join(", " + Environment.NewLine, GetMissingParameters())}");

            return RootNode.Resolve<T>(this);
        }

        private bool HasMissingParameters()
        {
            return GetMissingParameters().Count() > 0;
        }

        private IEnumerable<string> GetMissingParameters()
        {
            return RequiredParameters.Except(Parameters.Keys);
        }

        internal void AddNestedFormulaResult(string key, T result)
        {
            if (_nestedFormulaResults.ContainsKey(key))
                throw new InvalidOperationException($"The result for key [{key}] has already been cached");

            _nestedFormulaResults.Add(key, result);
        }
    }
}
