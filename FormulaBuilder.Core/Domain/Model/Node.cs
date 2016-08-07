using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain.Model
{
    public abstract class Node
    {
        public int Id { get; }
        public string Value { get; }
        public int Position { get; }
        public IEnumerable<Node> Children { get; }

        protected List<object> _resolvedChildren = new List<object>();

        protected internal Node(NodeEntity nodeEntity)
        {
            if (nodeEntity == null)
                throw new ArgumentNullException(nameof(nodeEntity));

            Id = nodeEntity.Id;
            Value = nodeEntity.Value;
            Position = nodeEntity.Position;
            Children = nodeEntity.Children.Select(ne => Node.Create(ne));
        }

        public static Node Create(NodeEntity nodeEntity)
        {
            var nodeType = nodeEntity.Type.Name;

            switch(nodeType)
            {
                case "operator":
                    return new OperationNode(nodeEntity);
                case "token":
                    return new ParameterNode(nodeEntity);
                default:
                    throw new InvalidOperationException($"Unrecognized node type [{nodeType}].");
            }
        }

        public abstract HashSet<string> GatherParameters();

        public abstract object Resolve(Formula formulaContext);
    }
}
