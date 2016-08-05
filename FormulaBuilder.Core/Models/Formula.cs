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
        public virtual NodeEntity RootNode { get; protected internal set; }

        protected Formula()
        {
        }

        public Formula(string name, NodeEntity rootNode)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (rootNode == null)
                throw new ArgumentNullException(nameof(rootNode));

            Name = name;
            RootNode = rootNode;
        }
    }
}
