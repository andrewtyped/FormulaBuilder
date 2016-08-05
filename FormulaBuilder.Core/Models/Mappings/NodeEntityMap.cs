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
            Id(x => x.Id).GeneratedBy.Assigned();
            References(x => x.Type)
                .Cascade.SaveUpdate()
                .Not.Nullable()
                .Column("TypeId");
            Map(x => x.Value)
                .Not.Nullable();
            Map(x => x.Position);
            HasManyToMany(m => m.Parents)
                .Table("ParentNodeChildNodeMapping")
                .ParentKeyColumn("ChildNodeId")
                .ChildKeyColumn("ParentNodeId")
                .Fetch.Subselect()
                .LazyLoad()
                .Cascade.None();           
            HasManyToMany(m => m.Children)
                .Table("ParentNodeChildNodeMapping")
                .ParentKeyColumn("ParentNodeId")
                .ChildKeyColumn("ChildNodeId")
                .Fetch.Subselect()
                .LazyLoad()
                .Cascade.None();
            //.OrderBy(nameof(NodeEntity.Position));
        }
    }
}
