using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain
{
    public class FormulaBuilder
    {

        public void Build(Formula formula)
        {
            //the top most node is the one that never appears in a links bottomnode
            var bottomNodes = formula.Links
                .Select(l => l.BottomNode.Id)
                .Distinct();

            var topNodes = formula.Links
                .Select(l => l.TopNode.Id)
                .Distinct();

            var topmostNode = topNodes.Except(bottomNodes).Single();

            throw new NotImplementedException();
        }

        public FormulaNode GetTopMostNode(Formula formula)
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
    }
}
