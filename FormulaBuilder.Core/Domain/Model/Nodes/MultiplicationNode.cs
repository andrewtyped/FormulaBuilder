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

        protected override T Aggregate<T>(IEnumerable<T> operands)
        {
            var product = Operator<T>.Zero;
            int i = 0;

            foreach (var operand in operands)
            {
                if (i == 0)
                    product = Operator<T>.Add(product, operand);
                else
                    product = Operator<T>.Multiply(product, operand);

                i++;
            }

            return product;
        }
    }
}
