using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Cache;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.DependencyResolution
{
    public class NHibernateRegistry : Registry
    {
        public NHibernateRegistry(string connectionString)
        {
            For<ISessionFactory>().Singleton().Use(ctx =>
            CreateSessionFactory(connectionString));

            For<ISession>().Use(ctx => ctx.GetInstance<ISessionFactory>().OpenSession());
        }

        public static NHibernate.Cfg.Configuration Configuration { get; private set; }

        public static ISessionFactory CreateSessionFactory(string connectionString)
        {
            Configuration = Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2012.ConnectionString(connectionString)
                    )
                .Cache(c => c.UseQueryCache().ProviderClass<HashtableCacheProvider>())
                .Mappings(mapCfg =>
                {
                    mapCfg.FluentMappings.AddFromAssemblyOf<NHibernateRegistry>();
                    mapCfg.FluentMappings.Conventions.Add(DefaultCascade.All(), ForeignKey.EndsWith("Id"));
                })
                .BuildConfiguration();

#if DEBUG
            try
            {
                return Configuration.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                //StructureMap tends to swallow errors coming from BuildSessionFactory.
                //Using debug assert to force errors to come through loud and clear.
                System.Diagnostics.Debug.Assert(1 == 0, ex.ToString());
                return null;
            }
#else
            return Configuration.BuildSessionFactory();
#endif

        }
    }
}
