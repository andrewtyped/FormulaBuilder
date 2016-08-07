using FormulaBuilder.Core.Domain.Model;
using FormulaBuilder.Core.Models;
using FormulaBuilder.Tests.DependencyResolution;
using FormulaBuilder.Tests.SqlLite;
using NHibernate;
using NHibernate.Linq;
using NUnit.Framework;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Tests
{

    [TestFixture]
    public class BaseUnitTest
    {
        private IContainer _container;
        protected IContainer _nestedContainer;
        protected ISession _session;
        public BaseUnitTest()
        {
            //_container = IoC.InitializeForUnitTests();
            //_nestedContainer = _container.GetNestedContainer();
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _container = IoC.InitializeForUnitTests();
            _nestedContainer = _container.GetNestedContainer();
        }

        [SetUp]
        public void SetUp()
        {
            _nestedContainer = _container.GetNestedContainer();
            _session = _nestedContainer.GetInstance<ISession>();
            TestData.InsertTestData(_session);
        }

        protected FormulaEntity GetTripleSumFormulaEntity()
        {
            var tripleSumFormulaEntity = _session.Query<FormulaEntity>()
               .Single(f => f.Name == "Triple Sum");

            return tripleSumFormulaEntity;
        }

        protected Formula GetTripleSumFormula()
        {
            var tripleSumFormulaEntity = _session.Query<FormulaEntity>()
                .Single(f => f.Name == "Triple Sum");
            return new Formula(tripleSumFormulaEntity);
        }


        [TearDown]
        public void TearDown()
        {
            if (_nestedContainer != null)
                _nestedContainer.Dispose();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (_nestedContainer != null)
                _nestedContainer.Dispose();

            if (_container != null)
                _container.Dispose();
        }
    }


}
