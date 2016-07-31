using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain
{
    public class FormulaParser
    {
        private readonly Formula _formula;
        private ExecutableFormulaBuilder _executableFormulaBuilder;
        public FormulaParser(Formula formula)
        {
            if (formula == null)
                throw new ArgumentNullException(nameof(formula));

            _formula = formula;
            _executableFormulaBuilder = ExecutableFormulaBuilder.Initialize();
        }
        public ExecutableFormula<decimal> Parse(Formula formula)
        {
            var rootNode = GetRootNode(formula);
            return Parse(rootNode);
        }

        internal ExecutableFormula<decimal> Parse(FormulaNode node)
        {
            _executableFormulaBuilder = _executableFormulaBuilder
                .EnterContext()
                .WithStep(node);

            var childNodes = GetChildNodes(node);
            foreach (var childNode in childNodes)
            {
                Parse(childNode);
            }

            return _executableFormulaBuilder
                .ExitContext()
                .Build<decimal>();
        }

        internal FormulaNode GetRootNode(Formula formula)
        {
            //the top most node is the one that never appears in a links bottomnode
            var rootNode = formula.Nodes.Single(n => n.Parent == null);
            return rootNode;
        }

        internal IEnumerable<FormulaNode> GetChildNodes(FormulaNode parentNode)
        {
            var childNodes = _formula.Nodes
                .Where(node => node.Parent != null)
                .Where(node => node.Parent.Id == parentNode.Id)
                .OrderBy(node => node.Position);

            return childNodes;
        }
    }
}
