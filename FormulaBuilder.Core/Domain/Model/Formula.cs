using FormulaBuilder.Core.Domain.Model.Nodes;
using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain.Model
{
    public class Formula
    {
        public int Id { get; }
        public string Name { get; }
        public Node RootNode { get; }

        public Formula(FormulaEntity formulaEntity)
        {
            if (formulaEntity == null)
                throw new ArgumentNullException(nameof(formulaEntity));

            Id = formulaEntity.Id;
            Name = formulaEntity.Name;
            RootNode = Node.Create(formulaEntity.RootNode);
        }

        protected Formula(int id, string name, Node rootNode)
        {
            Id = id;
            Name = name;
            RootNode = rootNode;
        }
    }
}
