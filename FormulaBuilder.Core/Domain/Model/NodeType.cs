using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain.Model
{
    public abstract class NodeType: IEquatable<NodeType>
    {
        public int Id { get; }
        public string Name { get; }

        protected NodeType(int id, string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            Id = id;
            Name = name;
        }

        public static NodeType Create(NodeTypeEntity nodeTypeEntity)
        {
            if (nodeTypeEntity == null)
                throw new ArgumentNullException(nameof(nodeTypeEntity));

            switch(nodeTypeEntity.Name)
            {
                case "operator":
                    return Operator;
                case "token":
                    return Token;
                default:
                    throw new InvalidOperationException($"Unrecognized {nameof(nodeTypeEntity)} with name {nodeTypeEntity.Name}");
            }
        }

        public static NodeType Operator => new OperatorNodeType();
        public static NodeType Token => new TokenNodeType();

        public override bool Equals(object other)
        {
            return Equals(other as NodeType);
        }

        public bool Equals(NodeType other)
        {
            if (other == null)
                return false;
            else if (GetType() != other.GetType())
                return false;
            else
                return Id == other.Id &&
                    Name == other.Name;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int prime1 = 499;
                int prime2 = 1063;
                int hash = prime1;
                hash = hash * prime2 + Id;
                hash = hash * prime2 + Name.GetHashCode();
                return hash;
            }
        }

        private class OperatorNodeType : NodeType
        {
            public OperatorNodeType()
                :base(1, "operator")
            {

            }
        }

        private class TokenNodeType : NodeType
        {
            public TokenNodeType()
                :base(2, "token")
            {

            }
        }
    }
}
