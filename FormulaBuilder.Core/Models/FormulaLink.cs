using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models
{
    public class FormulaLink
    {
        public virtual int Id { get; protected internal set; }
        public virtual Formula Formula { get; protected internal set; }
        public virtual string LinkType { get; protected internal set; }
        public virtual FormulaNode BottomNode { get; protected internal set; }
        public virtual FormulaNode TopNode { get; protected internal set; }

        protected FormulaLink()
        {
        }

        public FormulaLink(string linkType, FormulaNode bottomNode, FormulaNode topNode)
        {
            if (linkType == null)
                throw new ArgumentException(nameof(linkType));
            if (bottomNode == null)
                throw new ArgumentException(nameof(bottomNode));
            if (topNode == null)
                throw new ArgumentNullException(nameof(topNode));

            LinkType = linkType;
            BottomNode = bottomNode;
            TopNode = topNode;
        }
    }
}
