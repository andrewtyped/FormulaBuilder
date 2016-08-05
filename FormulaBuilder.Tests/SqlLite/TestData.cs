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
                var tripleSumFormula = CreateTripleSumFormula();
                //WHY CANT I SAVE WITHOUT GETTING DUPLICATES BACK????
                SaveNode(tripleSumFormula.RootNode, session);
                session.Save(tripleSumFormula);

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
            var rootNode = CreateTripleSumNodes();
            var formula = new Formula("Triple Sum", rootNode);
            return formula;
        }

        private static NodeEntity CreateTripleSumNodes()
        {
            var param1Node = new NodeEntity(1,TOKEN, PARAM1,0, new List<NodeEntity>());
            var param2Node = new NodeEntity(2,TOKEN, PARAM2,1, new List<NodeEntity>());
            var param3Node = new NodeEntity(3,TOKEN, PARAM3, 0, new List<NodeEntity>());
            var bottomPlusNode = new NodeEntity(4,OPERATOR, PLUS,1, new List<NodeEntity>()
            {
                param1Node,
                param2Node
            });
            var topPlusNode = new NodeEntity(5,OPERATOR,PLUS,0,new List<NodeEntity>()
            {
                bottomPlusNode,
                param3Node
            });

            return topPlusNode;
        }

        private static void SaveNode(NodeEntity node, ISession session)
        {
            foreach(var child in node.Children)
            {
                SaveNode(child, session);
            }

            session.SaveOrUpdate(node);
        }
    }
}
