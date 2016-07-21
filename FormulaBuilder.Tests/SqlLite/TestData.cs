using FormulaBuilder.Core.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Tests.SqlLite
{
    internal static class TestData
    {
        private const string OPERATOR = "operator";
        private const string TOKEN = "token";
        public static void InsertTestData(ISession session)
        {
            using (var tx = session.BeginTransaction())
            {
                foreach (var node in CreateFormulaNodes())
                {
                    session.Save(node);
                }

                session.Save(CreateTripleSumFormula());

                tx.Commit();
            }

            session.Clear();
        }

        private static List<FormulaNode> CreateFormulaNodes()
        {
            return new List<FormulaNode>()
            {
                Plus(),
                Param1(),
                Param2(),
                Param3()
            };
        }

        private static FormulaNode _plus;
        private static FormulaNode Plus()
        {
            return _plus ?? (_plus = new FormulaNode(OPERATOR, "+"));
        }

        private static FormulaNode _param1;
        private static FormulaNode Param1()
        {
            return  _param1 ?? (_param1 = new FormulaNode(TOKEN, "Param1"));
        }

        private static FormulaNode _param2;
        private static FormulaNode Param2()
        {
            return _param2 ?? (_param2 = new FormulaNode(TOKEN, "Param2"));
        }

        private static FormulaNode _param3;
        private static FormulaNode Param3()
        {
            return _param3 ?? (_param3 = new FormulaNode(TOKEN, "Param3"));
        }

        /// <summary>
        /// encapsulates the expression (+ (+ a b) c)
        /// </summary>
        /// <returns></returns>
        public static Formula CreateTripleSumFormula()
        {
            var formula = new Formula()
            {
                Name = "Triple Sum",
                Expressions = CreateTripleSumExpressions()
            };

            foreach(var expression in formula.Expressions)
            {
                expression.Formula = formula;
            }

            return formula;
        }

        private static List<FormulaExpression> CreateTripleSumExpressions()
        {
            var rootExpression = new FormulaExpression()
            {
                Parent = null,
                Operand1 = Plus(),
                Operand2 = Param3(),
                Operator = Plus()
            };

            var innerSumExpression = new FormulaExpression()
            {
                Parent = rootExpression,
                Operand1 = Param1(),
                Operand2 = Param2(),
                Operator = Plus()
            };

            return new List<FormulaExpression>()
            {
                rootExpression,
                innerSumExpression
            };
        }
    }
}
