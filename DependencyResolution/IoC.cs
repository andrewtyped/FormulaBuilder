using FormulaBuilder.Core.DependencyResolution;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize(string connectionString)
        {
            return new Container(cfg =>
            {
                cfg.AddRegistry(new NHibernateRegistry(connectionString));
                cfg.AddRegistry<LoggingRegistry>();
            });
        }
    }
}
