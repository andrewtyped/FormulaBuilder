using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FormulaBuilder.Core.Models;
using MiscUtil;

namespace FormulaBuilder.Core.Domain.Model.Nodes
{
    public abstract class OperationNode : Node
    {
        private static Dictionary<string, Func<NodeEntity, OperationNode>> _operationRegistry =
            new Dictionary<string, Func<NodeEntity, OperationNode>>()
            {
                {"+", (ne) => new AdditionNode(ne) },
                {"-", (ne) => new SubtractionNode(ne) },
                {"*", (ne) => new MultiplicationNode(ne) },
                {"/", (ne) => new DivisionNode(ne) }
            };
        protected internal OperationNode(NodeEntity nodeEntity) : base(nodeEntity)
        {
        }

        public static OperationNode CreateOperation(NodeEntity nodeEntity)
        {
            return _operationRegistry[nodeEntity.Value](nodeEntity);
        }

        public static void AddCustomOperation(OperationNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            else if (_operationRegistry.ContainsKey(node.Value))
                throw new InvalidOperationException($"An operation with key {node.Value} already exists.");

            _operationRegistry.Add(node.Value, (ne) => node);
        }

        public override HashSet<string> GatherParameters()
        {
            var parameters = new HashSet<string>();

            foreach(var child in Children)
            {
                parameters.UnionWith(child.GatherParameters());
            }

            return parameters;
        }

        public override T Resolve<T>(ExecutableFormula<T> formulaContext)
        {
            var resolvedChildren = ResolveChildren(formulaContext);
            var result = Aggregate(resolvedChildren);
            return result;
        }

        private IEnumerable<T> ResolveChildren<T>(ExecutableFormula<T> formula)
        {
            var resolvedChildren = new List<T>();

            foreach (var child in Children)
                resolvedChildren.Add(child.Resolve<T>(formula));

            return resolvedChildren;
        }

        protected abstract T Aggregate<T>(IEnumerable<T> operands);
    }
}
