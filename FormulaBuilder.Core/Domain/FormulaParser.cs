using FormulaBuilder.Core.Domain.Model;
using FormulaBuilder.Core.Models;
using System;

namespace FormulaBuilder.Core.Domain
{
    public class FormulaParser
    {
        private readonly Formula _formula;
        private ExecutableFormulaBuilder _executableFormulaBuilder;
        public FormulaParser(Formula formula)
        {
            if (formula == null)
                throw new ArgumentNullException(nameof(formula));

            _formula = formula;
            _executableFormulaBuilder = ExecutableFormulaBuilder.Initialize();
        }
        public ExecutableFormula<decimal> Parse(Formula formula)
        {
            var rootNode = formula.RootNode;
            return Parse(rootNode);
        }

        internal ExecutableFormula<decimal> Parse(Node node)
        {
            _executableFormulaBuilder = _executableFormulaBuilder
                .EnterContext()
                .WithStep(node);

            var childNodes = node.Children;

            foreach (var childNode in childNodes)
            {
                Parse(childNode);
            }

            return _executableFormulaBuilder
                .ExitContext()
                .Build<decimal>();
        }
    }
}
