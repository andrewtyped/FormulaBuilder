using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain.Model
{
    public abstract class Parameter
    {
        public string Name { get; }
        protected readonly object _value;

        protected Parameter() { }
        internal Parameter(string name, object value)
        {
            Name = name;
            _value = value;
        }

        public abstract object GetValue();
        public abstract Type GetParameterType();
    }

    public sealed class Parameter<T> : Parameter
        where T:struct
    {
        private readonly T _typedvalue;
        private Parameter() { }
        internal Parameter(string name, T value)
            :base(name, value)
        {
            _typedvalue = value;
        }

        public override object GetValue()
        {
            return GetTypedValue();
        }

        public T GetTypedValue()
        {
            return _typedvalue;
        }

        public override Type GetParameterType()
        {
            return typeof(T);
        }
    }
}
