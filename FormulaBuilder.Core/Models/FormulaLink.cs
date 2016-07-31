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
        public virtual int Position { get; protected internal set; }
        public virtual FormulaNode BottomNode { get; protected internal set; }
        public virtual FormulaNode TopNode { get; protected internal set; }

        protected FormulaLink()
        {
        }

        public FormulaLink(int position, FormulaNode bottomNode, FormulaNode topNode)
        {
            if (position < 0)
                throw new ArgumentException(nameof(position));
            if (bottomNode == null)
                throw new ArgumentNullException(nameof(bottomNode));
            if (topNode == null)
                throw new ArgumentNullException(nameof(topNode));

            Position = position;
            BottomNode = bottomNode;
            TopNode = topNode;
        }
    }
}
