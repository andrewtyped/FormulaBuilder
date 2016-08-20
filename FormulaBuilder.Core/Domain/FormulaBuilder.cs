using FormulaBuilder.Core.Domain.Model;
using FormulaBuilder.Core.Domain.Model.Nodes;
using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain
{
    public interface IFormulaIdBuilder
    {
        IFormulaNameBuilder WithId(int id);
    }

    public interface IFormulaNameBuilder
    {
        IFormulaRootNodeBuilder WithName(string name);
    }

    public interface IFormulaRootNodeBuilder
    {
        IFormulaPartsBuilder WithRootNode(INodeBuilder nodeBuilder);
    }

    public interface IFormulaBuilder
    {
        Formula Build();
    }

    public interface IFormulaPartsBuilder : IFormulaBuilder
    {
        IFormulaBuilder WithNestedFormulas(params IFormulaBuilder[] formulaBuilders);
    }

    

    public class FormulaBuilder :
        IFormulaIdBuilder,
        IFormulaNameBuilder,
        IFormulaRootNodeBuilder,
        IFormulaPartsBuilder,
        IFormulaBuilder
    {
        private int _id;
        private string _name;
        private INodeBuilder _rootNodeBuilder;
        private FormulaBuilder _parentFormulaBuilder;
        private List<IFormulaBuilder> _nestedFormulaBuilders;
        private FormulaBuilder(FormulaBuilder parentFormulaBuilder)
        {
            _parentFormulaBuilder = parentFormulaBuilder;
            _nestedFormulaBuilders = new List<IFormulaBuilder>();
        }

        public static IFormulaIdBuilder Formula()
        { 
            return new FormulaBuilder(null);
        }

        private static FormulaBuilder Formula(FormulaBuilder parentFormulaBuilder)
        {
            if (parentFormulaBuilder == null)
                throw new ArgumentNullException(nameof(parentFormulaBuilder));

            var formulaBuilder = new FormulaBuilder(parentFormulaBuilder);
            return formulaBuilder;
        }

        public IFormulaNameBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public IFormulaRootNodeBuilder WithName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"name must be non-null, non-empty, and non-whitespace", nameof(name));

            _name = name;

            return this;
        }

        public IFormulaBuilder WithNestedFormulas(params IFormulaBuilder[] formulaBuilders)
        {
            if (formulaBuilders == null)
                throw new ArgumentNullException(nameof(formulaBuilders));

            foreach (var formulaBuilder in formulaBuilders)
                _nestedFormulaBuilders.Add(formulaBuilder);

            return this;
        }

        public IFormulaPartsBuilder WithRootNode(INodeBuilder nodeBuilder)
        {
            if (nodeBuilder == null)
                throw new ArgumentNullException(nameof(nodeBuilder));

            _rootNodeBuilder = nodeBuilder;
            return this;
        }

        public Formula Build()
        {
            var nestedFormulas = _nestedFormulaBuilders.Select(formula => formula.Build());
            return new Formula(_id, _name, _rootNodeBuilder.Build(), nestedFormulas);
        }
    }
}
