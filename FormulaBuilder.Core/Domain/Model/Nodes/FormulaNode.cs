using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain.Model.Nodes
{
    class FormulaNode : Node
    {
        internal FormulaNode(NodeEntity nodeEntity)
            :base(nodeEntity)
        {
        }

        protected FormulaNode(NodeDTO nodeDTO)
            :base(nodeDTO)
        {
        }

        public static FormulaNode CreateFormulaNode(NodeEntity nodeEntity)
        {
            return new FormulaNode(nodeEntity);
        }

        public static FormulaNode CreateFormulaNode(NodeDTO nodeDTO)
        {
            return new FormulaNode(nodeDTO);
        }

        public override HashSet<string> GatherParameters(Formula formulaContext)
        {
            var nestedFormula = formulaContext.NestedFormulas[Value];
            return nestedFormula.RootNode.GatherParameters(formulaContext);
        }

        public override T Resolve<T>(ExecutableFormula<T> formulaContext)
        {
            T result;

            if (!formulaContext.NestedFormulaResults.TryGetValue(Value, out result))
            {
                var nestedFormula = formulaContext.NestedFormulas[Value];
                result = nestedFormula.RootNode.Resolve(formulaContext);
                formulaContext.AddNestedFormulaResult(Value, result);
            }

            return result;
        }
    }
}
