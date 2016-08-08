using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain.Model.Nodes
{
    public abstract class Node
    {
        public int Id { get; }
        public string Value { get; }
        public int Position { get; }
        public IEnumerable<Node> Children { get; }

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
                    return OperationNode.CreateOperation(nodeEntity);
                case "token":
                    return new ParameterNode(nodeEntity);
                default:
                    throw new InvalidOperationException($"Unrecognized node type [{nodeType}].");
            }
        }

        public abstract HashSet<string> GatherParameters();

        public abstract T Resolve<T>(ExecutableFormula<T> formulaContext);
    }
}
