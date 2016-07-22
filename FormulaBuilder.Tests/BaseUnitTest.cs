using FormulaBuilder.Tests.DependencyResolution;
using NHibernate;
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
            _container = IoC.InitializeForUnitTests();
            _nestedContainer = _container.GetNestedContainer();
        }

        [SetUp]
        public void SetUp()
        {
            _nestedContainer = _container.GetNestedContainer();
            _session = _nestedContainer.GetInstance<ISession>();
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
