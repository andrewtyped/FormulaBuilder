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

        private static BaseNode CreateTripleSumNodes()
        {
            var noChildren = new List<BaseNode>();
            var param1Node = BaseNode.Create(0, PARAMETER, PARAM1, noChildren);
            var param2Node = BaseNode.Create(0,PARAMETER, PARAM2, noChildren);
            var param3Node = BaseNode.Create(0, PARAMETER, PARAM3, noChildren);
            var bottomPlusNode = BaseNode.Create(0, OPERATOR, PLUS, new List<BaseNode>()
            {
                param1Node,
                param2Node
            });
            var topPlusNode = BaseNode.Create(0,OPERATOR,PLUS,new List<BaseNode>()
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

        public static BaseNode CreateGeneralGravityNodes()
        {
            var noChildren = new List<BaseNode>();
            var GravityConstant = BaseNode.Create(0,PARAMETER, "G", noChildren);
            var mass1 = BaseNode.Create(0,PARAMETER, "m1", noChildren);
            var mass2 = BaseNode.Create(0,PARAMETER, "m2", noChildren);
            var distance = BaseNode.Create(0,PARAMETER, "d", noChildren);

            var dividend = BaseNode.Create(0,OPERATOR, "*", new List<BaseNode>()
            {
                GravityConstant,
                mass1,
                mass2
            });
            var divisor = BaseNode.Create(0,OPERATOR, "*", new List<BaseNode>()
            {
                distance,
                distance
            });

            var quotient = BaseNode.Create(0,OPERATOR, "/", new List<BaseNode>()
            {
                dividend,
                divisor
            });

            return quotient;
        }

        private static void SaveNode(BaseNode node, ISession session)
        {
            foreach(var child in node.Children)
            {
                SaveNode(child, session);
            }

            session.SaveOrUpdate(node);
        }
    }
}
