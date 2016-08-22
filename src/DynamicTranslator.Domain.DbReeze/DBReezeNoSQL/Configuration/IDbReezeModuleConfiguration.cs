using DBreeze;

namespace DynamicTranslator.Domain.DbReeze.DBReezeNoSQL.Configuration
{
    public interface IDbReezeModuleConfiguration
    {
        DBreezeConfiguration Configuration { get; set; }
    }
}