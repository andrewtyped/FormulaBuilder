using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models.Mappings
{
    public class FormulaExpressionMap : ClassMap<FormulaExpression>
    {
        public FormulaExpressionMap()
        {
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.Formula)
                .Cascade.None()
                .Column("FormulaId");
            References(x => x.Parent)
                .Cascade.None()
                .Column("ParentId");
            References(x => x.Operand1)
                .Cascade.None()
                .Column("Operand1Id");
            References(x => x.Operand2)
                .Cascade.None()
                .Column("Operand2Id");
            References(x => x.Operator)
                .Cascade.None()
                .Column("OperatorId");
        }
    }
}
