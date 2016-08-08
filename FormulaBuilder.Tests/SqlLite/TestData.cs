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
                SaveNode(tripleSumFormula.RootNode, session);
                session.SaveOrUpdate(tripleSumFormula);

                var generalGravityFormula = CreateGeneralGravityFormula();
                SaveNode(generalGravityFormula.RootNode, session);
                session.SaveOrUpdate(generalGravityFormula);

                tx.Commit();
            }

            session.Clear();
        }

        /// <summary>
        /// encapsulates the Link (+ (+ a b) c)
        /// </summary>
        /// <returns></returns>
        public static FormulaEntity CreateTripleSumFormula()
        {
            var rootNode = CreateTripleSumNodes();
            var formula = new FormulaEntity("Triple Sum", rootNode);
            return formula;
        }

        private static NodeEntity CreateTripleSumNodes()
        {
            var param1Node = new NodeEntity(TOKEN, PARAM1,0, new List<NodeEntity>());
            var param2Node = new NodeEntity(TOKEN, PARAM2,1, new List<NodeEntity>());
            var param3Node = new NodeEntity(TOKEN, PARAM3, 0, new List<NodeEntity>());
            var bottomPlusNode = new NodeEntity(OPERATOR, PLUS,1, new List<NodeEntity>()
            {
                param1Node,
                param2Node
            });
            var topPlusNode = new NodeEntity(OPERATOR,PLUS,0,new List<NodeEntity>()
            {
                bottomPlusNode,
                param3Node
            });

            return topPlusNode;
        }

        public static FormulaEntity CreateGeneralGravityFormula()
        {
            var rootNode = CreateGeneralGravityNodes();
            var formula = new FormulaEntity("General Gravity", rootNode);
            return formula;
        }

        public static NodeEntity CreateGeneralGravityNodes()
        {
            var noChildren = new List<NodeEntity>();
            var GravityConstant = new NodeEntity(TOKEN, "G", 0, noChildren);
            var mass1 = new NodeEntity(TOKEN, "m1", 1, noChildren);
            var mass2 = new NodeEntity(TOKEN, "m2", 2, noChildren);
            var distance = new NodeEntity(TOKEN, "d", 0, noChildren);

            var dividend = new NodeEntity(OPERATOR, "*", 0, new List<NodeEntity>()
            {
                GravityConstant,
                mass1,
                mass2
            });
            var divisor = new NodeEntity(OPERATOR, "*", 1, new List<NodeEntity>()
            {
                distance,
                distance
            });

            var quotient = new NodeEntity(OPERATOR, "/", 0, new List<NodeEntity>()
            {
                dividend,
                divisor
            });

            return quotient;
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
