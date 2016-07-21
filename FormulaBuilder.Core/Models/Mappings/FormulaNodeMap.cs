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
            Map(x => x.Type).Not.Nullable().Length(50);
            Map(x => x.Value).Not.Nullable().Length(100);
        }
    }
}
