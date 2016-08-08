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

        protected override T Aggregate<T>(IEnumerable<T> operands)
        {
            var difference = Operator<T>.Zero;
            int i = 0;

            foreach (var operand in operands)
            {
                if (i == 0)
                    difference = Operator<T>.Add(difference, operand);
                else
                    difference = Operator<T>.Subtract(difference, operand);

                i++;
            }

            return difference;
        }
    }
}
