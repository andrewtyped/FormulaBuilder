using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaBuilder.Core.Models;

namespace FormulaBuilder.Core.Domain.Model
{
    public class OperationNode : Node
    {
        protected internal OperationNode(NodeEntity nodeEntity) : base(nodeEntity)
        {
        }

        public override HashSet<string> GatherParameters()
        {
            var parameters = new HashSet<string>();

            foreach(var child in Children)
            {
                parameters.UnionWith(child.GatherParameters());
            }

            return parameters;
        }

        public override object Resolve(Formula formulaContext)
        {
            foreach (var child in Children)
            {
                _resolvedChildren.Add(child.Resolve(formulaContext));
            }

            var sum = 0.0m;

            foreach (var operand in _resolvedChildren)
            {
                sum += (decimal)operand;
            }

            return sum;
        }
    }
}
