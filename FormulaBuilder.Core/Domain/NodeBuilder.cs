using FormulaBuilder.Core.Domain.Model.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FormulaBuilder.Core.Domain.Model.Nodes.NodeType;

namespace FormulaBuilder.Core.Domain
{
    public interface INodeTypeBuilder
    {
        INodePartsBuilder NestedFormula(string formulaName);
        INodePartsBuilder Operation(string operationName);
        INodePartsBuilder Parameter(string parameterName);
    }

    public interface INodePartsBuilder
    {
        INodePartsBuilder WithId(int id);
        INodeTypeBuilder WithChild();
        INodeEndBuilder EndNode();
    }

    public interface INodeEndBuilder
    {
        INodeEndBuilder EndNode();
        IFormulaPartsBuilder EndNodes();
        INodeTypeBuilder WithChild();
    }

    public interface INodeBuilder
    {
        Node Build();
    }

    public class NodeBuilder :
        INodeTypeBuilder,
        INodePartsBuilder,
        INodeEndBuilder,
        INodeBuilder
    {
        private readonly ExecutableFormulaBuilder _formulaBuilder;
        private readonly NodeBuilder _parentNodeBuilder;
        private readonly List<NodeBuilder> _childNodeBuilders;

        private int _childCounter;

        private int _id;
        private string _value;
        private NodeType _nodeType = EMPTY;
        private int _position;
        private NodeBuilder(ExecutableFormulaBuilder formulaBuilder, NodeBuilder parentNodeBuilder, int position = 0)
        {
            _formulaBuilder = formulaBuilder;
            _parentNodeBuilder = parentNodeBuilder;
            _childNodeBuilders = new List<NodeBuilder>();
            _nodeType = EMPTY;
            _position = position;
        }

        public static INodeTypeBuilder Initialize(ExecutableFormulaBuilder formulaBuilder)
        {
            if (formulaBuilder == null)
                throw new ArgumentNullException(nameof(FormulaBuilder));

            var nodeBuilder = new NodeBuilder(formulaBuilder, null);
            return nodeBuilder;
        }

        private static INodeTypeBuilder Initialize(ExecutableFormulaBuilder formulaBuilder, NodeBuilder parentNodeBuilder, int position)
        {
            if (formulaBuilder == null)
                throw new ArgumentNullException(nameof(FormulaBuilder));

            if (parentNodeBuilder == null)
                throw new ArgumentNullException(nameof(parentNodeBuilder));

            var nodeBuilder = new NodeBuilder(formulaBuilder, parentNodeBuilder, position);
            return nodeBuilder;
        }

        public INodeTypeBuilder WithChild()
        {
            var childNodeBuilder = Initialize(_formulaBuilder, this, _childCounter++);
            _childNodeBuilders.Add(childNodeBuilder as NodeBuilder);
            return childNodeBuilder;
        }

        public INodePartsBuilder NestedFormula(string formulaName)
        {
            _nodeType = FORMULA;
            SetNodeValue(formulaName);
            return this;
        }

        public INodePartsBuilder Operation(string operationName)
        {
            _nodeType = OPERATOR;
            SetNodeValue(operationName);
            return this;
        }
        
        public INodePartsBuilder Parameter(string parameterName)
        {
            _nodeType = PARAMETER;
            SetNodeValue(parameterName);
            return this;
        }

        private void SetNodeValue(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _value = value;
        }

        public INodePartsBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public INodeEndBuilder EndNode()
        {
            return _parentNodeBuilder ?? this;
        }

        public IFormulaPartsBuilder EndNodes()
        {
            return _formulaBuilder;
        }

        public Node Build()
        {
            var children = _childNodeBuilders.Select(child => child.Build());
            var nodeDTO = new NodeDTO(_id, _value, _position, children, _nodeType);
            var node = Node.Create(nodeDTO);

            return node;
        }
    }
}
