using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models
{
    public class NodeType
    {
        public virtual int Id { get; protected internal set; }
        public virtual string Name { get; protected internal set; }
        protected NodeType() { }

        internal NodeType(int id, string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            Id = id;
            Name = name;
        } 
    }
}
