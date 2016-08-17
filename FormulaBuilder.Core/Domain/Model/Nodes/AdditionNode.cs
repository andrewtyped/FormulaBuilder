using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain.Model.Nodes
{
    internal class AdditionNode : OperationNode
    {
        internal AdditionNode(NodeEntity nodeEntity)
            : base(nodeEntity)
        {

        }

        internal AdditionNode(NodeDTO nodeDTO)
            :base(nodeDTO)
        {

        }

        protected override decimal AggregateDecimal(IEnumerable<decimal> operands)
        {
            return operands.Sum();
        }

        protected override double AggregateDouble(IEnumerable<double> operands)
        {
            return operands.Sum();
        }

        protected override float AggregateFloat(IEnumerable<float> operands)
        {
            return operands.Sum();
        }
    }
}
