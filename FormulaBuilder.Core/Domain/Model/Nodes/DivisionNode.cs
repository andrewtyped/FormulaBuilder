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

        protected override T Aggregate<T>(IEnumerable<T> operands)
        {
            var quotient = Operator<T>.Zero;
            int i = 0;

            foreach (var operand in operands)
            {
                if (i == 0)
                    quotient = Operator<T>.Add(quotient, operand);
                else
                    quotient = Operator<T>.Divide(quotient, operand);

                i++;
            }

            return quotient;
        }
    }
}
