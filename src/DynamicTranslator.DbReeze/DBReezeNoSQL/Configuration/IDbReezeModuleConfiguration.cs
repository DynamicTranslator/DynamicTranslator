using DBreeze;

namespace DynamicTranslator.DbReeze.DBReezeNoSQL.Configuration
{
    public interface IDbReezeModuleConfiguration
    {
        DBreezeConfiguration Configuration { get; set; }
    }
}