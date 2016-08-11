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
        INodeTypeBuilder WithRootNode();
    }

    public interface IFormulaPartsBuilder
    {
        IFormulaIdBuilder WithNestedFormula();
        IFormulaPartsBuilder EndNestedFormula();
        IFormulaBuilder EndNestedFormulas();
        ExecutableFormula<T> Build<T>();
    }

    public interface IFormulaBuilder
    {
        ExecutableFormula<T> Build<T>();
    }

    public class ExecutableFormulaBuilder :
        IFormulaIdBuilder,
        IFormulaNameBuilder,
        IFormulaRootNodeBuilder,
        IFormulaPartsBuilder,
        IFormulaBuilder
    {
        private int _id;
        private string _name;
        private NodeBuilder _rootNodeBuilder;
        private ExecutableFormulaBuilder _parentFormulaBuilder;
        private List<IFormulaBuilder> _nestedFormulaBuilders;
        private ExecutableFormulaBuilder(ExecutableFormulaBuilder parentFormulaBuilder)
        {
            _parentFormulaBuilder = parentFormulaBuilder;
            _nestedFormulaBuilders = new List<IFormulaBuilder>();
        }

        public static IFormulaIdBuilder Initialize()
        { 
            return new ExecutableFormulaBuilder(null);
        }

        private static ExecutableFormulaBuilder Initialize(ExecutableFormulaBuilder parentFormulaBuilder)
        {
            if (parentFormulaBuilder == null)
                throw new ArgumentNullException(nameof(parentFormulaBuilder));

            var formulaBuilder = new ExecutableFormulaBuilder(parentFormulaBuilder);
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

        public IFormulaIdBuilder WithNestedFormula()
        {
            var nestedFormulaBuilder = Initialize(this);
            _nestedFormulaBuilders.Add(nestedFormulaBuilder as IFormulaBuilder);

            return nestedFormulaBuilder;
        }

        public IFormulaPartsBuilder EndNestedFormula()
        {
            return _parentFormulaBuilder ?? this;
        }
        public IFormulaBuilder EndNestedFormulas()
        {
            if (_parentFormulaBuilder != null)
                throw new InvalidOperationException("A parent formula exists. Cannot end nested formula construction while parent formulas exist");

            return this;
        }
        public INodeTypeBuilder WithRootNode()
        {
            _rootNodeBuilder = NodeBuilder.Initialize(this) as NodeBuilder;
            return _rootNodeBuilder;
        }

        public ExecutableFormula<T> Build<T>()
        {
            var nestedFormulas = _nestedFormulaBuilders.Select(formula => formula.Build<T>());
            return new ExecutableFormula<T>(_id, _name, _rootNodeBuilder.Build(), nestedFormulas);
        }
    }
}
