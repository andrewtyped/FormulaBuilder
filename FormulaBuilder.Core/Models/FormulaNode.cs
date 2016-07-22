using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models
{
    public class FormulaNode
    {
        public virtual int Id { get; protected internal set; }
        public virtual Node Node { get; protected internal set; }

        protected FormulaNode()
        {
        }

        public FormulaNode(Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            Node = node;
        }
    }
}
