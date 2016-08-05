using FormulaBuilder.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        internal ExecutableFormula<decimal> Parse(NodeEntity node)
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
