using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain.Model
{
    public abstract class Number
    {
        public static implicit operator decimal(Number number)
        {
            return (decimal)number.GetValue();
        }

        public static implicit operator double(Number number)
        {
            return (double)number.GetValue();
        }

        public static implicit operator float(Number number)
        {
            return (float)number.GetValue();
        }

        public static implicit operator Number(decimal number)
        {
            return new AssignedNumber(number);
        }

        public static implicit operator Number(double number)
        {
            return new AssignedNumber(number);
        }

        public static implicit operator Number(float number)
        {
            return new AssignedNumber(number);
        }

        internal abstract object GetValue();
    }

    internal class AssignedNumber : Number
    {
        private readonly decimal? _decimal;
        private readonly double? _double;
        private readonly float? _float;
        public AssignedNumber(decimal value)
        {
            _decimal = value;
        }

        public AssignedNumber(double value)
        {
            _double = value;
        }

        public AssignedNumber(float value)
        {
            _float = value;
        }

        internal override object GetValue()
        {
            var value = _decimal ?? _double ?? (object)_float;

            if (value == null)
                throw new InvalidOperationException("Number value was never assigned");

            return value;

        }
    }
}
