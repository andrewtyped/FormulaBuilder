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
                foreach (var node in CreateNodes())
                {
                    session.Save(node);
                }

                session.Save(CreateTripleSumFormula());

                tx.Commit();
            }

            session.Clear();
        }

        private static List<Node> CreateNodes()
        {
            return new List<Node>()
            {
                Plus(),
                Param1(),
                Param2(),
                Param3()
            };
        }

        private static Node _plus;
        private static Node Plus()
        {
            return _plus ?? (_plus = new Node(OPERATOR, "+"));
        }

        private static Node _param1;
        private static Node Param1()
        {
            return  _param1 ?? (_param1 = new Node(TOKEN, "Param1"));
        }

        private static Node _param2;
        private static Node Param2()
        {
            return _param2 ?? (_param2 = new Node(TOKEN, "Param2"));
        }

        private static Node _param3;
        private static Node Param3()
        {
            return _param3 ?? (_param3 = new Node(TOKEN, "Param3"));
        }

        /// <summary>
        /// encapsulates the Link (+ (+ a b) c)
        /// </summary>
        /// <returns></returns>
        public static Formula CreateTripleSumFormula()
        {
            var links = CreateTripleSumLinks();
            var formula = new Formula("Triple Sum", links);
            return formula;
        }

        private static List<FormulaLink> CreateTripleSumLinks()
        {
            var topPlusNode = new FormulaNode(Plus());
            var bottomPlusNode = new FormulaNode(Plus());
            var param1Node = new FormulaNode(Param1());
            var param2Node = new FormulaNode(Param2());
            var param3Node = new FormulaNode(Param3());
            var topPlusBottomPlusLink = new FormulaLink(0, bottomPlusNode, topPlusNode);
            var topPlusParam3Link = new FormulaLink(1, param3Node, topPlusNode);
            var bottomPlusParam1Link = new FormulaLink(0, param1Node, bottomPlusNode);
            var bottomPlusParam2Link = new FormulaLink(1, param2Node, bottomPlusNode);

            return new List<FormulaLink>()
            {
                topPlusBottomPlusLink,
                topPlusParam3Link,
                bottomPlusParam1Link,
                bottomPlusParam2Link
            };
        }
    }
}
