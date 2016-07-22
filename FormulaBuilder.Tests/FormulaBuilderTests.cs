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
            using (var session = _nestedContainer.GetInstance<ISession>())
            {
                TestData.InsertTestData(session);

                session.Flush();

                var formulas = session.Query<Formula>().ToList();
                var link = session.Query<FormulaLink>().ToList();
            }
        }
    }
}
