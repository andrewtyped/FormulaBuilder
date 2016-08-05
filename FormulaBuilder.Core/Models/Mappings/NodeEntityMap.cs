using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models.Mappings
{
    class NodeEntityMap : ClassMap<NodeEntity>
    {
        public NodeEntityMap()
        {
            Schema("fb");
            Table("Node");
            Id(x => x.Id).GeneratedBy.Assigned();
            References(x => x.Type)
                .Cascade.SaveUpdate()
                .Not.Nullable()
                .Column("NodeTypeId");
            Map(x => x.Value)
                .Not.Nullable();
            Map(x => x.Position);
            References(m => m.Parent)
                .Cascade.None()
                .Column("ParentNodeId")
                .LazyLoad()
                .Cascade.None();
            HasMany(m => m.Children)
                .KeyColumn("ParentNodeId")
                .Fetch.Subselect()
                .LazyLoad()
                .Cascade.None();
            //.OrderBy(nameof(NodeEntity.Position));
        }
    }
}
