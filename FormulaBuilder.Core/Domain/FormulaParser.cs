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
                .WithStep(node.Node);

            var linksToNode = GetLinksTo(node);
            foreach (var link in linksToNode)
            {
                Parse(link.BottomNode);
            }


            return _executableFormulaBuilder
                .ExitContext()
                .Build<decimal>();
        }

        internal FormulaNode GetRootNode(Formula formula)
        {
            //the top most node is the one that never appears in a links bottomnode
            var bottomNodes = formula.Links
                .Select(l => l.BottomNode.Id)
                .Distinct();

            var topNodes = formula.Links
                .Select(l => l.TopNode.Id)
                .Distinct();

            var topmostNodeId = topNodes.Except(bottomNodes).Single();

            var topmostNode = formula.Links.Where(l => l.TopNode.Id == topmostNodeId).Select(l => l.TopNode).FirstOrDefault();

            return topmostNode;
        }

        internal IEnumerable<FormulaLink> GetLinksTo(FormulaNode node)
        {
            var childLinks = _formula.Links
                .Where(link => link.TopNode.Id == node.Id)
                .OrderBy(link => link.LinkType);

            return childLinks;
        }
    }
}
