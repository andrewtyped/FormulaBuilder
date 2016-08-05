using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models.Mappings
{
    public class FormulaMap : ClassMap<Formula>
    {
        public FormulaMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Not.Nullable().Length(200);
            References(x => x.RootNode).Cascade.None().Column("RootNodeId");
        }
    }
}
