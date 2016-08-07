using FormulaBuilder.Core.Domain.Model;
using MiscUtil;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain
{
    public interface IParameterNameBuilder
    {
        IParameterValueBuilder WithName(string name);
    }

    public interface IParameterValueBuilder
    {
        IParameterBuilder WithValue<T>(T value)
            where T : struct;
    }

    public interface IParameterBuilder
    {
        Parameter Build();
    }

    public class ParameterBuilder:
        IParameterNameBuilder,
        IParameterValueBuilder,
        IParameterBuilder
    {
        internal string _name;
        internal Type _type;
        internal object _value;
        private static HashSet<Type> _supportedTypes = new HashSet<Type>()
        {
            typeof(decimal),
            typeof(double),
            typeof(float),
            typeof(int),
            typeof(long)
        };

        private ParameterBuilder()
        {

        }
        public static IParameterNameBuilder Initialize()
        {
            return new ParameterBuilder();
        }

        public IParameterValueBuilder WithName(string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentException("name must be non-null, non-empty, non-whitespace", nameof(parameterName));

            _name = parameterName;
            return this;
        }

        public IParameterBuilder WithValue<T>(T value)
            where T:struct
        {
            var type = typeof(T);
            if(_supportedTypes.Contains(type) == false)
                throw new NotSupportedException($"{type.Name} is not supported. Supported types are { string.Join(", " + Environment.NewLine, _supportedTypes.Select(st => st.Name))}");

            _type = type;
            _value = value;

            return this;
        }

        public Parameter Build()
        {
            var strongParameterType = typeof(Parameter<>).MakeGenericType(_type);
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var parameter = (Parameter)Activator.CreateInstance(
                strongParameterType, 
                flags, 
                null, 
                new object[] { _name, _value }, 
                CultureInfo.InvariantCulture
            );

            return parameter;
        }
    }
}
