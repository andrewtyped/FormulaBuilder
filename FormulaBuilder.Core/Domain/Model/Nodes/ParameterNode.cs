using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaBuilder.Core.Models;
using MiscUtil;

namespace FormulaBuilder.Core.Domain.Model.Nodes
{
    class ParameterNode : Node
    {
        protected internal ParameterNode(NodeEntity nodeEntity) : base(nodeEntity)
        {
        }

        protected internal ParameterNode(NodeDTO nodeDTO) : base(nodeDTO)
        {

        }
        protected internal static ParameterNode CreateParameterNode(NodeEntity nodeEntity)
        {
            return new ParameterNode(nodeEntity);
        }
        protected internal static ParameterNode CreateParameterNode(NodeDTO nodeDTO)
        {
            return new ParameterNode(nodeDTO);
        }

        public override HashSet<string> GatherParameters()
        {
            return new HashSet<string>() { this.Value };
        }

        public override T Resolve<T>(ExecutableFormula<T> formulaContext)
        {
            var parameter = formulaContext.Parameters[Value];
            var value = parameter.GetValue();
            var type = parameter.GetParameterType();

            if (type == typeof(T))
                return (T)value;
            else
            {
                var convertMethod = typeof(Operator).GetMethod(nameof(Operator.Convert));
                var genericConvertMethod = convertMethod.MakeGenericMethod(type, typeof(T));
                var result = (T)genericConvertMethod.Invoke(null, new object[] { value });
                return result;
            }
        }
    }
}
