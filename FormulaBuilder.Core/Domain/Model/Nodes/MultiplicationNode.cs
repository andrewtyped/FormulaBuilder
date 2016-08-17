using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil;

namespace FormulaBuilder.Core.Domain.Model.Nodes
{
    internal class MultiplicationNode : OperationNode
    {
        internal MultiplicationNode(NodeEntity nodeEntity)
            : base(nodeEntity)
        {

        }

        internal MultiplicationNode(NodeDTO nodeDTO)
            : base(nodeDTO)
        {

        }

        protected override decimal AggregateDecimal(IEnumerable<decimal> operands)
        {
            decimal result = 1.00m;

            foreach(var operand in operands)
            {
                result *= operand;
            }

            return result;
        }

        protected override double AggregateDouble(IEnumerable<double> operands)
        {
            double result = 1.00d;

            foreach (var operand in operands)
            {
                result *= operand;
            }

            return result;
        }

        protected override float AggregateFloat(IEnumerable<float> operands)
        {
            float result = 1.00f;

            foreach (var operand in operands)
            {
                result *= operand;
            }

            return result;
        }
    }
}
