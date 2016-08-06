using FormulaBuilder.Core.Domain;
using FormulaBuilder.Core.Domain.Model;
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
        public void Can_Execute_Triple_Sum_Formula()
        {
            var tripleSumFormulaEntity = _session.Query<FormulaEntity>().Single(f => f.Name == "Triple Sum");
            var tripleSumFormula = new Formula(tripleSumFormulaEntity);

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
