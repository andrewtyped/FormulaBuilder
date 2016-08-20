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
        public BaseNode RootNode { get; }
        public HashSet<string> RequiredParameters { get; }
        public Dictionary<string, Formula> NestedFormulas { get; }

        protected internal Formula(FormulaEntity formulaEntity)
        {
            if (formulaEntity == null)
                throw new ArgumentNullException(nameof(formulaEntity));

            Id = formulaEntity.Id;
            Name = formulaEntity.Name;
            RootNode = BaseNode.Create(formulaEntity.RootNode);
        }

        protected internal Formula(int id, string name, BaseNode rootNode)
            :this(id, name, rootNode, new List<Formula>())
        {
        }

        protected internal Formula(int id, string name, BaseNode rootNode, IEnumerable<Formula> nestedFormulas)
        {
            Id = id;
            Name = name;
            RootNode = rootNode;

            NestedFormulas = new Dictionary<string, Formula>();

            foreach (var formula in nestedFormulas)
            {
                FlattenFormula(formula);
            }

            //Very important to wait until formulas are resolved to get parameters, since the
            //formula dictionary will be used by any formula nodes to get parameters.
            RequiredParameters = rootNode.GatherParameters(this);
        }

        private void FlattenFormula(Formula formula)
        {
            if (NestedFormulas.ContainsKey(formula.Name) == false)
                NestedFormulas.Add(formula.Name, formula);

            foreach (var nestedformula in formula.NestedFormulas.Values)
            {
                FlattenFormula(formula);
            }
        }
    }
}
