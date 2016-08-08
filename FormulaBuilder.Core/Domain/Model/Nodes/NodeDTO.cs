using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain.Model.Nodes
{
    public class NodeDTO
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public int Position { get; set; }
        public IEnumerable<Node> Children { get; set; }

        public NodeType NodeType { get; set; }

        public NodeDTO(int id, string value, int position, IEnumerable<Node> children, NodeType nodeType)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"value must be non-null, non-empty, non-whitespace.", nameof(value));
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            Id = id;
            Value = value;
            Position = position;
            Children = children.OrderBy(n => n.Position);
            NodeType = nodeType;
        }
    }
}
