using FormulaBuilder.Core.Domain.Model.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models
{
    public class NodeTypeEntity
    {
        public virtual int Id { get; protected internal set; }
        public virtual string Name { get; protected internal set; }
        protected NodeTypeEntity() { }

        internal NodeTypeEntity(int id, string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            Id = id;
            Name = name;
        } 

        internal static NodeTypeEntity Operation = new NodeTypeEntity(1, "operation");
        internal static NodeTypeEntity Parameter = new NodeTypeEntity(2, "parameter");

        internal static NodeTypeEntity Create(Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (node is OperationNode)
            {
                return Operation;
            }
            else if (node is ParameterNode)
            {
                return Parameter;
            }
            else
                throw new InvalidOperationException();
        }
    }
}
