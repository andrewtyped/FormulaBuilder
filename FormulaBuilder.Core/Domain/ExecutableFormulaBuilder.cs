using FormulaBuilder.Core.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain
{
    public interface IFormulaSetter
    {
        IReturnTypeSetter Formula(Formula formula);
    }

    public interface IReturnTypeSetter
    {
        IParameterSetter Returns<T>();
    }

    public interface IParameterSetter
    {
        IParameterSetter WithParameter(Parameter parameter);
        IExecutableFormulaBuilder AndNoMoreParameters();
    }

    public interface IExecutableFormulaBuilder
    {
        Executable<T> Build<T>();
    }

    public class ExecutableFormulaBuilder:
        IFormulaSetter,
        IReturnTypeSetter,
        IParameterSetter,
        IExecutableFormulaBuilder
    {
        private Formula _formulaDefinition;
        private readonly Dictionary<string, Parameter> _parameters = new Dictionary<string, Parameter>();
        private ExecutableFormulaBuilder()
        {

        }

        public static IFormulaSetter Executable()
        {
            return new ExecutableFormulaBuilder();
        }

        public IReturnTypeSetter Formula(Formula formula)
        {
            if (formula == null)
                throw new ArgumentNullException(nameof(formula));

            _formulaDefinition = formula;

            return this;
        }

        public IParameterSetter Returns<T>()
        {
            return this;
        }

        public IParameterSetter WithParameter(Parameter parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (_formulaDefinition.RequiredParameters.Contains(parameter.Name) == false)
                throw new InvalidOperationException($"Formula doesn't require a parameter named {parameter.Name}. Expected parameter names are {string.Join(", " + Environment.NewLine, _formulaDefinition.RequiredParameters)}");

            _parameters.Add(parameter.Name, parameter);

            return this;
        }

        public IExecutableFormulaBuilder AndNoMoreParameters()
        {
            if (HasMissingParameters())
                throw new InvalidOperationException($"The following required parameters are missing: {string.Join(", " + Environment.NewLine, GetMissingParameters())}");

            return this;
        }

        private bool HasMissingParameters()
        {
            return GetMissingParameters().Count() > 0;
        }

        private IEnumerable<string> GetMissingParameters()
        {
            return _formulaDefinition.RequiredParameters.Except(_parameters.Keys);
        }

        public Executable<T> Build<T>()
        {
            return new Executable<T>(
                _formulaDefinition,
                _parameters
            );
        }
    }
}
