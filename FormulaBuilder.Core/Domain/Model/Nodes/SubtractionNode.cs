using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil;

namespace FormulaBuilder.Core.Domain.Model.Nodes
{
    internal class SubtractionNode : OperationNode
    {
        internal SubtractionNode(NodeEntity nodeEntity)
            : base(nodeEntity)
        {

        }

        internal SubtractionNode(NodeDTO nodeDTO)
            : base(nodeDTO)
        {

        }

        protected override decimal AggregateDecimal(IEnumerable<decimal> operands)
        {
            var head = operands.FirstOrDefault();
            var tail = operands.Skip(1);
            var tailSum = tail.Sum();

            return head - tailSum;
        }

        protected override double AggregateDouble(IEnumerable<double> operands)
        {
            var head = operands.FirstOrDefault();
            var tail = operands.Skip(1);
            var tailSum = tail.Sum();

            return head - tailSum;
        }

        protected override float AggregateFloat(IEnumerable<float> operands)
        {
            var head = operands.FirstOrDefault();
            var tail = operands.Skip(1);
            var tailSum = tail.Sum();

            return head - tailSum;
        }
    }
}
