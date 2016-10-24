using DBreeze;

namespace DynamicTranslator.Domain.DbReeze.DBReezeNoSQL.Configuration
{
    public class DbReezeModuleConfiguration : IDbReezeModuleConfiguration
    {
        public DbReezeModuleConfiguration()
        {
            Configuration = new DBreezeConfiguration();
        }

        public DBreezeConfiguration Configuration { get; set; }
    }
}
