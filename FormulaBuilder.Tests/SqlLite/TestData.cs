using FormulaBuilder.Core.Domain;
using FormulaBuilder.Core.Domain.Model;
using FormulaBuilder.Core.Domain.Model.Nodes;
using FormulaBuilder.Core.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FormulaBuilder.Core.Domain.Model.Nodes.NodeType;

namespace FormulaBuilder.Tests.SqlLite
{
    internal static class TestData
    {
        private const string PLUS = "+";
        private const string PARAM1 = "Param1";
        private const string PARAM2 = "Param2";
        private const string PARAM3 = "Param3";
        public static void InsertTestData(ISession session)
        {
            var repository = new FormulaRepository(session);
            var tripleSumFormula = CreateTripleSumFormula();
            var generalGravityFormula = CreateGeneralGravityFormula();
            repository.Save(tripleSumFormula);
            repository.Save(generalGravityFormula);
        }

        /// <summary>
        /// encapsulates the Link (+ (+ a b) c)
        /// </summary>
        /// <returns></returns>
        public static Formula CreateTripleSumFormula()
        {
            var rootNode = CreateTripleSumNodes();
            var formula = new Formula(0,"Triple Sum", rootNode);
            return formula;
        }

        private static Node CreateTripleSumNodes()
        {
            var noChildren = new List<Node>();
            var param1Node = Node.Create(0, PARAMETER, PARAM1, 0, noChildren);
            var param2Node = Node.Create(0,PARAMETER, PARAM2,1, noChildren);
            var param3Node = Node.Create(0, PARAMETER, PARAM3, 0, noChildren);
            var bottomPlusNode = Node.Create(0, OPERATOR, PLUS,1, new List<Node>()
            {
                param1Node,
                param2Node
            });
            var topPlusNode = Node.Create(0,OPERATOR,PLUS,0,new List<Node>()
            {
                bottomPlusNode,
                param3Node
            });

            return topPlusNode;
        }

        public static Formula CreateGeneralGravityFormula()
        {
            var rootNode = CreateGeneralGravityNodes();
            var formula = new Formula(0,"General Gravity", rootNode);
            return formula;
        }

        public static Node CreateGeneralGravityNodes()
        {
            var noChildren = new List<Node>();
            var GravityConstant = Node.Create(0,PARAMETER, "G", 0, noChildren);
            var mass1 = Node.Create(0,PARAMETER, "m1", 1, noChildren);
            var mass2 = Node.Create(0,PARAMETER, "m2", 2, noChildren);
            var distance = Node.Create(0,PARAMETER, "d", 0, noChildren);

            var dividend = Node.Create(0,OPERATOR, "*", 0, new List<Node>()
            {
                GravityConstant,
                mass1,
                mass2
            });
            var divisor = Node.Create(0,OPERATOR, "*", 1, new List<Node>()
            {
                distance,
                distance
            });

            var quotient = Node.Create(0,OPERATOR, "/", 0, new List<Node>()
            {
                dividend,
                divisor
            });

            return quotient;
        }

        private static void SaveNode(Node node, ISession session)
        {
            foreach(var child in node.Children)
            {
                SaveNode(child, session);
            }

            session.SaveOrUpdate(node);
        }
    }
}
