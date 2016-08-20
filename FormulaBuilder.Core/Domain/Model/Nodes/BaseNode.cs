using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain.Model.Nodes
{
    public abstract class BaseNode
    {
        public int Id { get; }
        public string Value { get; }
        public IEnumerable<BaseNode> Children { get; }

        internal BaseNode(NodeEntity nodeEntity)
        {
            if (nodeEntity == null)
                throw new ArgumentNullException(nameof(nodeEntity));

            Id = nodeEntity.Id;
            Value = nodeEntity.Value;
            Children = nodeEntity.Children
                .OrderBy(ne => ne.Position)
                .Select(ne => BaseNode.Create(ne));
        }

        protected BaseNode(NodeDTO nodeDTO)
        {
            if (nodeDTO == null)
                throw new ArgumentNullException(nameof(nodeDTO));

            Id = nodeDTO.Id;
            Value = nodeDTO.Value;
            Children = nodeDTO.Children;
        }

        protected internal static BaseNode Create(NodeEntity nodeEntity)
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

        public static BaseNode Create(NodeDTO nodeDTO)
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

        public static BaseNode Create(int id, NodeType nodeType, string value, IEnumerable<BaseNode> children)
        {
            var nodeDTO = new NodeDTO(id, value, children, nodeType);
            return Create(nodeDTO);
        }

        public abstract HashSet<string> GatherParameters(Formula formulaContext);

        public abstract decimal Resolve(Executable<decimal> formulaContext);
        public abstract double Resolve(Executable<double> formulaContext);
        public abstract float Resolve(Executable<float> formulaContext);
    }
}
