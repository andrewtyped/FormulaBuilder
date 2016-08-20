using FormulaBuilder.Core.Domain.Model;
using FormulaBuilder.Core.Domain.Model.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models
{
    public class NodeEntity
    {
        public virtual int Id { get; protected internal set; }
        public virtual FormulaEntity FormulaReference { get; protected internal set; }
        public virtual NodeTypeEntity Type { get; protected internal set; }

        public virtual string Value { get; protected internal set; }
        public virtual int Position { get; protected internal set; }

        public virtual IList<NodeEntity> Children { get; protected internal set; }

        protected internal virtual NodeEntity Parent { get; protected set; }


        protected NodeEntity()
        {
        }

        public NodeEntity(NodeTypeEntity type, string value, int position, IList<NodeEntity> children)
            :this(null, type, value, position, children)
        {
        }

        public NodeEntity(NodeTypeEntity type, string value, int position, FormulaEntity formulaReference)
            :this(formulaReference,type,value,position,new List<NodeEntity>())
        {
        }

        private NodeEntity(FormulaEntity formulaReference, NodeTypeEntity type, string value, int position, IList<NodeEntity> children)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            FormulaReference = formulaReference;
            Type = type;
            Value = value;
            Position = position;
            Children = children;

            foreach (var child in children)
            {
                child.Parent = this;
            }
        }


        internal NodeEntity(Formula formula, BaseNode node, int position)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            Id = node.Id;
            Type = NodeTypeEntity.Create(node);
            Value = node.Value;
            Position = position;
            Children = node.Children.Select((n, idx) => new NodeEntity(formula, n, idx)).ToList();

            Formula referencedFormula;

            if (formula.NestedFormulas.TryGetValue(node.Value, out referencedFormula))
                FormulaReference = new FormulaEntity(referencedFormula);

            foreach(var child in Children)
            {
                child.Parent = this;
            }
        }
    }
}
