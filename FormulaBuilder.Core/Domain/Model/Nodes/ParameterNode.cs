using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaBuilder.Core.Models;

namespace FormulaBuilder.Core.Domain.Model.Nodes
{
    class ParameterNode : BaseNode
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

        public override HashSet<string> GatherParameters(Formula formulaContext)
        {
            return new HashSet<string>() { this.Value };
        }

        public override decimal Resolve(Executable<decimal> formulaContext)
        {
            return ResolveGeneric(formulaContext);
        }

        public override double Resolve(Executable<double> formulaContext)
        {
            return ResolveGeneric(formulaContext);
        }

        public override float Resolve(Executable<float> formulaContext)
        {
            return ResolveGeneric(formulaContext);
        }

        private T ResolveGeneric<T>(Executable<T> formulaContext) where T : struct
        {
            var parameter = formulaContext.Parameters[Value];
            var result = (parameter as Parameter<T>)?.GetTypedValue();

            if(!result.HasValue)
                throw new InvalidOperationException($"Parameter type {parameter.GetParameterType().Name} does not equal expected type {typeof(T).Name}");

            return result.Value;
        }


    }
}
