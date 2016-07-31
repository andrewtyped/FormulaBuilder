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
    }
}
