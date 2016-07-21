using FormulaBuilder.Tests.DependencyResolution;
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

        public BaseUnitTest()
        {
            _container = IoC.InitializeForUnitTests();
            _nestedContainer = _container.GetNestedContainer();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            if (_nestedContainer != null)
                _nestedContainer.Dispose();

            if (_container != null)
                _container.Dispose();
        }
    }


}
