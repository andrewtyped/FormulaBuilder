using FormulaBuilder.Core.Domain.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
        IDecimalParameterSetter ReturnsDecimal();
        IDoubleParameterSetter ReturnsDouble();
        IFloatParameterSetter ReturnsFloat();
    }

    public interface IDecimalParameterSetter : IExecutableFormulaBuilder
    {
        IDecimalParameterSetter WithParameter(string name, decimal value);
    }

    public interface IDoubleParameterSetter : IExecutableFormulaBuilder
    {
        IDoubleParameterSetter WithParameter(string name, double value);
    }

    public interface IFloatParameterSetter : IExecutableFormulaBuilder
    {
        IFloatParameterSetter WithParameter(string name, float value);
    }

    public interface IExecutableFormulaBuilder
    {
        Executable Build();
    }

    public class ExecutableFormulaBuilder:
        IFormulaSetter,
        IReturnTypeSetter,
        IDecimalParameterSetter,
        IDoubleParameterSetter,
        IFloatParameterSetter
    {
        private Formula _formulaDefinition;
        private readonly Dictionary<string, Parameter> _parameters = new Dictionary<string, Parameter>();
        private Type _returnType;
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

        public IDecimalParameterSetter ReturnsDecimal()
        {
            _returnType = typeof(decimal);
            return this;
        }

        public IDoubleParameterSetter ReturnsDouble()
        {
            _returnType = typeof(double);
            return this;
        }

        public IFloatParameterSetter ReturnsFloat()
        {
            _returnType = typeof(float);
            return this;
        }

        public IDecimalParameterSetter WithParameter(string name, decimal value)
        {
            AddParameter(name, value);
            return this;
        }

        public IDoubleParameterSetter WithParameter(string name, double value)
        {
            AddParameter(name, value);
            return this;
        }

        public IFloatParameterSetter WithParameter(string name, float value)
        {
            AddParameter(name, value);
            return this;
        }

        private void AddParameter<T>(string parameterName, T value)
            where T : struct
        {
            if (parameterName == null)
                throw new ArgumentNullException(nameof(parameterName));

            if (_formulaDefinition.RequiredParameters.Contains(parameterName) == false)
            {
                throw new InvalidOperationException($"Formula doesn't require a parameter named {parameterName}. Expected parameter names are " +
                    string.Join(", " + Environment.NewLine, _formulaDefinition.RequiredParameters)
                );
            }

            _parameters.Add(parameterName, new Parameter<T>(parameterName, value));
        }

        public Executable Build()
        {
            if (HasMissingParameters())
                throw new InvalidOperationException($"The following required parameters are missing: {string.Join(", " + Environment.NewLine, GetMissingParameters())}");

            var executableType = typeof(Executable<>);
            var genericExecutableType = executableType.MakeGenericType(_returnType);

            return  (Executable)Activator.CreateInstance(genericExecutableType, 
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,  
                new object[] { _formulaDefinition, _parameters as IReadOnlyDictionary<string, Parameter> }, 
                CultureInfo.CurrentCulture
           );
        }

        private bool HasMissingParameters()
        {
            return GetMissingParameters().Count() > 0;
        }

        private IEnumerable<string> GetMissingParameters()
        {
            return _formulaDefinition.RequiredParameters.Except(_parameters.Keys);
        }
    }
}
