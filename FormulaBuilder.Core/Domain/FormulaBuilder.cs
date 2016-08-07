using FormulaBuilder.Core.Domain.Model;
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
        IFormulaParameterBuilder WithRootNode(NodeEntity nodeEntity);
    }

    public interface IFormulaParameterBuilder
    {
        IFormulaParameterBuilder WithParameter(Parameter parameter);
        IFormulaReturnTypeBuilder AndNoMoreParameters();
    }

    public interface IFormulaReturnTypeBuilder
    {
        IFormulaBuilder WithReturnType<T>();
    }

    public interface IFormulaBuilder
    {
        Formula Build();
    }

    public class FormulaBuilderImpl :
        IFormulaIdBuilder,
        IFormulaNameBuilder,
        IFormulaRootNodeBuilder,
        IFormulaParameterBuilder,
        IFormulaReturnTypeBuilder,
        IFormulaBuilder
    {
        private int _id;
        private string _name;
        private Node _rootNode;
        private HashSet<string> _requiredParameters;
        private readonly Dictionary<string, Parameter> _parameters = new Dictionary<string, Parameter>();
        private Type _returnType;
        private FormulaBuilderImpl()
        {
        }

        public static IFormulaIdBuilder Initialize()
        {
            return new FormulaBuilderImpl();
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

        public IFormulaParameterBuilder WithRootNode(NodeEntity nodeEntity)
        {
            if (nodeEntity == null)
                throw new ArgumentNullException(nameof(nodeEntity));

            _rootNode = Node.Create(nodeEntity);
            _requiredParameters = _rootNode.GatherParameters();

            return this;
        }

        public IFormulaParameterBuilder WithParameter(Parameter parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (_requiredParameters.Contains(parameter.Name) == false)
                throw new InvalidOperationException($"Formula doesn't require a parameter named {parameter.Name}. Expected parameter names are {string.Join(", " + Environment.NewLine, _requiredParameters)}");

            _parameters.Add(parameter.Name, parameter);

            return this;
        }

        public IFormulaReturnTypeBuilder AndNoMoreParameters()
        {
            return this;
        }

        public IFormulaBuilder WithReturnType<T>()
        {
            _returnType = typeof(T);
            return this;
        }

        public Formula Build()
        {
            return new Formula(_id, _name, _rootNode, _parameters, _returnType);
        }
    }
}
