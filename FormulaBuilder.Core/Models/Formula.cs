using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models
{
    public class Formula
    {
        public virtual int Id { get; protected internal set; }
        public virtual string Name { get; protected internal set; }

        public virtual ICollection<FormulaNode> Nodes { get; protected internal set; }

        protected Formula()
        {
            Nodes = new List<FormulaNode>();
        }

        public Formula(string name)
        {
            Nodes = new List<FormulaNode>();

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        protected internal virtual void AddNode(FormulaNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            Nodes.Add(node);
        }

    }
}
