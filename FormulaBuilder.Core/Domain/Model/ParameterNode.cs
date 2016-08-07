using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaBuilder.Core.Models;

namespace FormulaBuilder.Core.Domain.Model
{
    class ParameterNode : Node
    {
        protected internal ParameterNode(NodeEntity nodeEntity) : base(nodeEntity)
        {
        }

        public override HashSet<string> GatherParameters()
        {
            return new HashSet<string>() { this.Value };
        }

        public override object Resolve(Formula formulaContext)
        {
            return formulaContext.Parameters[Value].GetValue();
        }
    }
}
