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

        internal Node(NodeEntity nodeEntity)
        {
            if (nodeEntity == null)
                throw new ArgumentNullException(nameof(nodeEntity));

            Id = nodeEntity.Id;
            Value = nodeEntity.Value;
            Position = nodeEntity.Position;
            Children = nodeEntity.Children.Select(ne => Node.Create(ne))
                .OrderBy(n => n.Position);
        }

        protected Node(NodeDTO nodeDTO)
        {
            if (nodeDTO == null)
                throw new ArgumentNullException(nameof(nodeDTO));

            Id = nodeDTO.Id;
            Value = nodeDTO.Value;
            Position = nodeDTO.Position;
            Children = nodeDTO.Children;
        }

        protected internal static Node Create(NodeEntity nodeEntity)
        {
            var nodeType = nodeEntity.Type.Name;

            switch (nodeType)
            {
                case "operation":
                    return OperationNode.CreateOperation(nodeEntity);
                case "parameter":
                    return ParameterNode.CreateParameterNode(nodeEntity);
                case "formula":
                    return FormulaNode.CreateFormulaNode(nodeEntity);
                default:
                    throw new InvalidOperationException($"Unrecognized node type [{nodeType}].");
            }
        }

        public static Node Create(NodeDTO nodeDTO)
        {
            if (nodeDTO == null)
                throw new ArgumentNullException(nameof(nodeDTO));

            switch (nodeDTO.NodeType)
            {
                case NodeType.OPERATOR:
                    return OperationNode.CreateOperation(nodeDTO);
                case NodeType.PARAMETER:
                    return ParameterNode.CreateParameterNode(nodeDTO);
                case NodeType.FORMULA:
                    return FormulaNode.CreateFormulaNode(nodeDTO);
                default:
                    throw new InvalidOperationException($"Unrecognized node type [{nodeDTO.NodeType.ToString()}].");
            }
        }

        public static Node Create(int id, NodeType nodeType, string value, int position, IEnumerable<Node> children)
        {
            var nodeDTO = new NodeDTO(id, value, position, children, nodeType);
            return Create(nodeDTO);
        }

        public abstract HashSet<string> GatherParameters(Formula formulaContext);

        public abstract T Resolve<T>(ExecutableFormula<T> formulaContext);
    }
}
