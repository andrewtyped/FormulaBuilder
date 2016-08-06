﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models
{
    public class NodeEntity
    {
        public virtual int Id { get; protected internal set; }
        public virtual NodeTypeEntity Type { get; protected internal set; }

        public virtual string Value { get; protected internal set; }
        public virtual int Position { get; protected internal set; }

        public virtual IList<NodeEntity> Children { get; protected internal set; }

        protected internal virtual NodeEntity Parent { get; protected set; }


        protected NodeEntity()
        {
        }

        public NodeEntity(NodeTypeEntity type, string value, int position, IList<NodeEntity> children)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (children == null)
                throw new ArgumentNullException(nameof(children));

            Type = type;
            Value = value;
            Position = position;
            Children = children;

            foreach (var child in children)
            {
                child.Parent = this;
            }
        }
    }
}
