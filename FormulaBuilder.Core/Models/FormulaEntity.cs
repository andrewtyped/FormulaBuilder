using FormulaBuilder.Core.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models
{
    public class FormulaEntity
    {
        public virtual int Id { get; protected internal set; }
        public virtual string Name { get; protected internal set; }
        public virtual NodeEntity RootNode { get; protected internal set; }

        protected FormulaEntity()
        {
        }

        public FormulaEntity(string name, NodeEntity rootNode)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (rootNode == null)
                throw new ArgumentNullException(nameof(rootNode));

            Name = name;
            RootNode = rootNode;
        }

        public FormulaEntity(Formula formula)
        {
            if (formula == null)
                throw new ArgumentNullException(nameof(formula));

            Id = formula.Id;
            Name = formula.Name;
            RootNode = new NodeEntity(formula, formula.RootNode);
        }
    }
}
