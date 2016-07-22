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
        [Test]
        public void Can_Persist_TestData()
        {
                TestData.InsertTestData(_session);

                var formulas = _session.Query<Formula>().ToList();
                var link = _session.Query<FormulaLink>().ToList();
        }

        [Test]
        public void Can_Get_Topmost_Node_Of_Formula()
        {
                TestData.InsertTestData(_session);

                var tripleSumFormula = _session.Query<Formula>().Single(f => f.Name == "Triple Sum");
                var builder = new Core.Domain.FormulaBuilder();
                var topmostNode = builder.GetTopMostNode(tripleSumFormula);

                Assert.That(topmostNode.Node.Value == "+");
        }
    }
}
