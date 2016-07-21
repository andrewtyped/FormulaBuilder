using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models
{
    public class FormulaExpression
    {
        public virtual int Id { get; set; }
        public virtual Formula Formula { get; set; }
        public virtual FormulaExpression Parent { get; set; }
        public virtual FormulaNode Operand1 { get; set; }
        public virtual FormulaNode Operand2 { get; set; }
        public virtual FormulaNode Operator { get; set; }
    }
}
