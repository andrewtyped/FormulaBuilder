using FormulaBuilder.Core.Domain;
using FormulaBuilder.Core.Models;
using FormulaBuilder.Tests.SqlLite;
using NHibernate;
using NHibernate.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Tests
{
    [TestFixture]
    public class FormulaBuilderTests : BaseUnitTest
    {
        //private Formula tripleSumFormula;
        //private FormulaParser parser;

        [SetUp]
        public void FormulaBuilderSetup()
        {
            TestData.InsertTestData(_session);
        }

        [Test]
        public void Can_Get_Root_Node_Of_Formula()
        {
            var tripleSumFormula = _session.Query<Formula>().Single(f => f.Name == "Triple Sum");
            var builder = new FormulaParser(tripleSumFormula);
            var rootNode = builder.GetRootNode(tripleSumFormula);

            Assert.That(rootNode.Node.Value == "+");
        }

        [Test]
        public void Can_Get_Links_To_Node()
        {
            var tripleSumFormula = _session.Query<Formula>().Single(f => f.Name == "Triple Sum");
            var builder = new FormulaParser(tripleSumFormula);
            var rootNode = builder.GetRootNode(tripleSumFormula);

            var linksToRootNode = builder.GetLinksTo(rootNode);

            Assert.AreEqual(2,linksToRootNode.Count());

            foreach (var link in linksToRootNode)
            {
                Assert.AreEqual(rootNode, link.TopNode);
            }
        
        }

        [Test]
        public void Links_Are_Sorted_By_Position()
        {
            var tripleSumFormula = _session.Query<Formula>().Single(f => f.Name == "Triple Sum");
            var parser = new FormulaParser(tripleSumFormula);
            var rootNode = parser.GetRootNode(tripleSumFormula);

            var linksToRootNode = parser.GetLinksTo(rootNode).ToList();
            Assert.That(linksToRootNode, Is.Ordered.By(nameof(FormulaLink.LinkType)));
        }

        [Test]
        public void Can_Execute_Triple_Sum_Formula()
        {
            var tripleSumFormula = _session.Query<Formula>().Single(f => f.Name == "Triple Sum");
            var parser = new FormulaParser(tripleSumFormula);
            var executable = parser.Parse(tripleSumFormula);

            var tokens = new Dictionary<string, decimal>()
            {
                {"Param1", 1.00m },
                {"Param2", 2.00m },
                {"Param3", 3.00m }
            };

            var result = executable.Execute(tokens);
            Assert.That(result == 6.00m);
            
        }
    }
}
