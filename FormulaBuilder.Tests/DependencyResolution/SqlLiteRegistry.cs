using FormulaBuilder.Tests.SqlLite;
using NHibernate;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Tests.DependencyResolution
{
    public class SqlLiteRegistry : Registry
    {
        public SqlLiteRegistry()
        {
            For<ISessionFactory>().Singleton().Use(InMemorySessionFactoryProvider.Instance.Initialize());
            For<ISession>().Use(InMemorySessionFactoryProvider.Instance.OpenSession());
        }
    }
}
