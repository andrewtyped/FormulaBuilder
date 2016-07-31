using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models
{
    public class Node
    {
        public virtual int Id { get; protected internal set; }
        public virtual NodeType Type { get; protected internal set; }
        public virtual string Value { get; protected internal set; }

        protected Node()
        {
        }

        public Node(NodeType type, string value)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Type = type;
            Value = value;
        }
    }
}
