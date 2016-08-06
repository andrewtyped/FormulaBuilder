using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models.Mappings
{
    public class FormulaEntityMap : ClassMap<FormulaEntity>
    {
        public FormulaEntityMap()
        {
            Schema("fb");
            Table("Formula");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Not.Nullable().Length(200);
            References(x => x.RootNode).Cascade.None().Column("RootNodeId");
        }
    }
}
