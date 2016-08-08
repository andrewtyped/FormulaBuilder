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
        IFormulaBuilder WithRootNode(Node node);
    }

    public interface IFormulaBuilder
    {
        ExecutableFormula<T> Build<T>();
    }

    public class ExecutableFormulaBuilder :
        IFormulaIdBuilder,
        IFormulaNameBuilder,
        IFormulaRootNodeBuilder,
        IFormulaBuilder
    {
        private int _id;
        private string _name;
        private Node _rootNode;
        private ExecutableFormulaBuilder()
        {
        }

        public static IFormulaIdBuilder Initialize()
        {
            return new ExecutableFormulaBuilder();
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

        public IFormulaBuilder WithRootNode(Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            _rootNode = node;
            return this;
        }

        public ExecutableFormula<T> Build<T>()
        {
            return new ExecutableFormula<T>(_id, _name, _rootNode);
        }
    }
}
