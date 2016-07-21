using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models
{
    public class Formula
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        public virtual IList<FormulaExpression> Expressions { get; set; }

        public Formula()
        {
            //Expressions = new List<FormulaExpression>();
        }
    }
}
