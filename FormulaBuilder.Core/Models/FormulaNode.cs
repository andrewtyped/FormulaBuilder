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
        public virtual Formula Formula { get; protected internal set; }
        public virtual FormulaNode Parent { get; protected internal set; }
        public virtual NodeTypeEntity Type { get; protected internal set; }

        public virtual string Value { get; protected internal set; }
        public virtual int Position { get; protected internal set; }


        protected FormulaNode()
        {
        }

        public FormulaNode(Formula formula, FormulaNode parent, NodeTypeEntity type, string value, int position)
        {
            if (formula == null)
                throw new ArgumentNullException(nameof(formula));
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Formula = formula;
            Parent = parent;
            Type = type;
            Value = value;
            Position = position;

            formula.AddNode(this);
        }
    }
}
