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

    public interface INodePartsBuilder : INodeBuilder
    {
        INodePartsBuilder WithId(int id);
        INodeBuilder WithChildren(params INodeBuilder[] nodeBuilders);
    }

    public interface INodeBuilder
    {
        BaseNode Build();
    }

    public class NodeBuilder :
        INodeTypeBuilder,
        INodePartsBuilder,
        INodeBuilder
    {
        private readonly List<INodeBuilder> _childNodeBuilders;

        private int _id;
        private string _value;
        private NodeType _nodeType = EMPTY;

        private NodeBuilder()
        {
            _childNodeBuilders = new List<INodeBuilder>();
            _nodeType = EMPTY;
        }

        public static INodeTypeBuilder Node
        {
            get
            {
                var nodeBuilder = new NodeBuilder();
                return nodeBuilder;
            }
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

        public INodeBuilder WithChildren(params INodeBuilder[] nodeBuilders)
        {
            if (nodeBuilders == null)
                throw new ArgumentNullException(nameof(nodeBuilders));

            foreach (var nodeBuilder in nodeBuilders)
            {
                _childNodeBuilders.Add(nodeBuilder);
            }

            return this;
        }

        public BaseNode Build()
        {
            var children = _childNodeBuilders.Select(child => child.Build());
            var nodeDTO = new NodeDTO(_id, _value, children, _nodeType);
            var node = Model.Nodes.BaseNode.Create(nodeDTO);

            return node;
        }
    }
}
