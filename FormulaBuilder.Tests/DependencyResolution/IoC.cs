using FormulaBuilder.Core.DependencyResolution;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Tests.DependencyResolution
{
    public static class IoC
    {
        private static object _locker = new object();
        private static IContainer _unitTestContainer;
        private static IContainer _integrationTestContainer;

        public static IContainer InitializeForUnitTests()
        {
            lock (_locker)
            {
                _unitTestContainer = _unitTestContainer ?? new Container(
                    c =>
                    {
                        c.AddRegistry<SqlLiteRegistry>();
                    });

                var debugStructureMapAssemblyReport = _unitTestContainer.WhatDoIHave();
                var debugStructureMapScansReport = _unitTestContainer.WhatDidIScan();
            }

            return _unitTestContainer;
        }

        public static IContainer InitializeForIntegrationTests(string connectionString)
        {
            lock (_locker)
            {
                _integrationTestContainer = _integrationTestContainer ?? new Container(
                    c =>
                    {
                        c.AddRegistry(new NHibernateRegistry(connectionString));
                    });

                var debugStructureMapAssemblyReport = _integrationTestContainer.WhatDoIHave();
                var debugStructureMapScansReport = _integrationTestContainer.WhatDidIScan();
            }

            return _integrationTestContainer;
        }
    }
}
