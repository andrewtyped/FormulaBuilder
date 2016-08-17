using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil;

namespace FormulaBuilder.Core.Domain.Model.Nodes
{
    internal class DivisionNode : OperationNode
    {
        internal DivisionNode(NodeEntity nodeEntity)
            : base(nodeEntity)
        {

        }

        internal DivisionNode(NodeDTO nodeDTO)
            :base(nodeDTO)
        {

        }

        protected override decimal AggregateDecimal(IEnumerable<decimal> operands)
        {
            decimal result = operands.FirstOrDefault();

            foreach(var operand in operands.Skip(1))
            {
                result /= operand;
            }

            return result;
        }

        protected override double AggregateDouble(IEnumerable<double> operands)
        {
            double result = operands.FirstOrDefault();

            foreach (var operand in operands.Skip(1))
            {
                result /= operand;
            }

            return result;
        }

        protected override float AggregateFloat(IEnumerable<float> operands)
        {
            float result = operands.FirstOrDefault();

            foreach (var operand in operands.Skip(1))
            {
                result /= operand;
            }

            return result;
        }
    }
}
