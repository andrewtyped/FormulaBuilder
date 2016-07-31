using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models.Mappings
{
    class FormulaNodeMap : ClassMap<FormulaNode>
    {
        public FormulaNodeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.Formula)
                .Cascade.None()
                .Not.Nullable()
                .Column("FormulaId");
            References(x => x.Parent)
                .Cascade.None()
                .Nullable()
                .Column("ParentId");
            References(x => x.Type)
                .Cascade.SaveUpdate()
                .Not.Nullable()
                .Column("TypeId");
            Map(x => x.Value)
                .Not.Nullable();
            Map(x => x.Position);
        }
    }
}
