using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FormulaBuilder.Core.Models;

namespace FormulaBuilder.Core.Domain.Model.Nodes
{
    public abstract class OperationNode : Node
    {
        private static Dictionary<string, Func<NodeDTO, OperationNode>> _operationRegistry =
            new Dictionary<string, Func<NodeDTO, OperationNode>>()
            {
                {"+", (nd) => new AdditionNode(nd) },
                {"-", (nd) => new SubtractionNode(nd) },
                {"*", (nd) => new MultiplicationNode(nd) },
                {"/", (nd) => new DivisionNode(nd) }
            };
        protected internal OperationNode(NodeEntity nodeEntity) : base(nodeEntity)
        {
        }

        public OperationNode(NodeDTO nodeDTO)
            :base(nodeDTO)
        {

        }

        internal static OperationNode CreateOperation(NodeEntity nodeEntity)
        {
            var nodeDTO = new NodeDTO(
                nodeEntity.Id,
                nodeEntity.Value,
                nodeEntity.Position,
                nodeEntity.Children.Select(ne => Node.Create(ne)),
                NodeType.OPERATOR);

            return _operationRegistry[nodeEntity.Value](nodeDTO);
        }

        public static OperationNode CreateOperation(NodeDTO nodeDTO)
        {
            return _operationRegistry[nodeDTO.Value](nodeDTO);
        }

        public static void AddCustomOperation(OperationNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            else if (_operationRegistry.ContainsKey(node.Value))
                throw new InvalidOperationException($"An operation with key {node.Value} already exists.");

            _operationRegistry.Add(node.Value, (ne) => node);
        }

        public override HashSet<string> GatherParameters(Formula formulaContext)
        {
            var parameters = new HashSet<string>();

            foreach(var child in Children)
            {
                parameters.UnionWith(child.GatherParameters(formulaContext));
            }

            return parameters;
        }

        public override decimal Resolve(Executable<decimal> formulaContext)
        {
            var resolvedChildren = ResolveChildrenDecimal(formulaContext);
            var result = AggregateDecimal(resolvedChildren);
            return result;
        }

        public override double Resolve(Executable<double> formulaContext)
        {
            var resolvedChildren = ResolveChildrenDouble(formulaContext);
            var result = AggregateDouble(resolvedChildren);
            return result;
        }

        public override float Resolve(Executable<float> formulaContext)
        {
            var resolvedChildren = ResolveChildrenFloat(formulaContext);
            var result = AggregateFloat(resolvedChildren);
            return result;
        }

        private IEnumerable<decimal> ResolveChildrenDecimal(Executable<decimal> formula)
        {
            var resolvedChildren = new List<decimal>();

            foreach (var child in Children)
                resolvedChildren.Add(child.Resolve(formula));

            return resolvedChildren;
        }

        private IEnumerable<double> ResolveChildrenDouble(Executable<double> formula)
        {
            var resolvedChildren = new List<double>();

            foreach (var child in Children)
                resolvedChildren.Add(child.Resolve(formula));

            return resolvedChildren;
        }

        private IEnumerable<float> ResolveChildrenFloat(Executable<float> formula)
        {
            var resolvedChildren = new List<float>();

            foreach (var child in Children)
                resolvedChildren.Add(child.Resolve(formula));

            return resolvedChildren;
        }

        protected abstract decimal AggregateDecimal(IEnumerable<decimal> operands);
        protected abstract double AggregateDouble(IEnumerable<double> operands);
        protected abstract float AggregateFloat(IEnumerable<float> operands);
    }
}
