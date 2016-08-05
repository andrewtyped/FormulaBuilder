using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models.Mappings
{
    class NodeTypeEntityMap : ClassMap<NodeTypeEntity>
    {
        public NodeTypeEntityMap()
        {
            Schema("fb");
            Table("NodeType");
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Name).Not.Nullable();
        }
    }
}
