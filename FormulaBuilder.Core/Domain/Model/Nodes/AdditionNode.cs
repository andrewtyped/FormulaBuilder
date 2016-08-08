using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiscUtil;

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

        protected override T Aggregate<T>(IEnumerable<T> operands)
        {
            var sum = Operator<T>.Zero;

            foreach (var operand in operands)
                sum = Operator<T>.Add(sum, operand);

            return sum;
        }
    }
}
