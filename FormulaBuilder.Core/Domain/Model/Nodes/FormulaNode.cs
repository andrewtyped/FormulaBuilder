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


        public override decimal Resolve(Executable<decimal> formulaContext)
        {
            decimal result;

            if (HasNoCachedResult(formulaContext, out result))
            {
                result = GetNestedFormula(formulaContext).RootNode.Resolve(formulaContext);
                formulaContext.AddNestedFormulaResult(Value, result);
            }

            return result;
        }

        public override double Resolve(Executable<double> formulaContext)
        {
            double result;

            if (HasNoCachedResult(formulaContext, out result))
            {
                result = GetNestedFormula(formulaContext).RootNode.Resolve(formulaContext);
                formulaContext.AddNestedFormulaResult(Value, result);
            }

            return result;
        }

        public override float Resolve(Executable<float> formulaContext)
        {
            float result;

            if (HasNoCachedResult(formulaContext, out result))
            {
                result = GetNestedFormula(formulaContext).RootNode.Resolve(formulaContext);
                formulaContext.AddNestedFormulaResult(Value, result);
            }

            return result;
        }

        private bool HasNoCachedResult<T>(Executable<T> formulaContext, out T result) where T:struct
        {
            return !formulaContext.NestedFormulaResults.TryGetValue(Value, out result);
        }

        private Formula GetNestedFormula<T>(Executable<T> formulaContext) where T:struct
        {
            return formulaContext.Formula.NestedFormulas[Value];
        }
    }
}
