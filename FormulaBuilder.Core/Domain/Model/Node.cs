using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain.Model
{
    public class Node
    {
        public int Id { get; }
        public NodeType Type { get; }
        public string Value { get; }
        public int Position { get; }
        public IEnumerable<Node> Children { get; }

        public Node(NodeEntity nodeEntity)
        {
            if (nodeEntity == null)
                throw new ArgumentNullException(nameof(nodeEntity));

            Id = nodeEntity.Id;
            Type = NodeType.Create(nodeEntity.Type);
            Value = nodeEntity.Value;
            Position = nodeEntity.Position;
            Children = nodeEntity.Children.Select(ne => new Node(ne));
        }
    }
}
