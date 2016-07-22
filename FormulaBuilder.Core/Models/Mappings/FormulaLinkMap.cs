using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models.Mappings
{
    public class FormulaLinkMap : ClassMap<FormulaLink>
    {
        public FormulaLinkMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.Formula)
                .Cascade.None()
                .Column("FormulaId");
            Map(x => x.LinkType);
            References(x => x.BottomNode)
                .Cascade.All()
                .Column("BottomNodeId");
            References(x => x.TopNode)
                .Cascade.All()
                .Column("TopNodeId");
        }
    }
}
