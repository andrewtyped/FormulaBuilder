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
        private static NodeTypeEntity OPERATOR = new NodeTypeEntity(1,"operator");
        private static NodeTypeEntity TOKEN = new NodeTypeEntity(2,"token");
        private const string PLUS = "+";
        private const string PARAM1 = "Param1";
        private const string PARAM2 = "Param2";
        private const string PARAM3 = "Param3";
        public static void InsertTestData(ISession session)
        {
            using (var tx = session.BeginTransaction())
            {
                session.Save(CreateTripleSumFormula());

                tx.Commit();
            }

            session.Clear();
        }

        /// <summary>
        /// encapsulates the Link (+ (+ a b) c)
        /// </summary>
        /// <returns></returns>
        public static Formula CreateTripleSumFormula()
        {
            var formula = new Formula("Triple Sum");
            var nodes = CreateTripleSumNodes(formula);
            return formula;
        }

        private static List<FormulaNode> CreateTripleSumNodes(Formula formula)
        {
            var topPlusNode = new FormulaNode(formula, null, OPERATOR,PLUS, 0);
            var bottomPlusNode = new FormulaNode(formula, topPlusNode, OPERATOR, PLUS,0);
            var param1Node = new FormulaNode(formula, bottomPlusNode, TOKEN, PARAM1, 0);
            var param2Node = new FormulaNode(formula, bottomPlusNode, TOKEN, PARAM2, 0);
            var param3Node = new FormulaNode(formula, topPlusNode, TOKEN, PARAM3, 0);

            return new List<FormulaNode>()
            {
                topPlusNode,
                bottomPlusNode,
                param1Node,
                param2Node,
                param3Node,
            };
        }
    }
}
