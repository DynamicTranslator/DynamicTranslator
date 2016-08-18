using System.Management;

using Abp.Extensions;

namespace DynamicTranslator.Configuration.UniqueIdentifier
{
    public class CpuBasedIdentifierProvider : IUniqueIdentifierProvider
    {
        public string Get()
        {
            var cpuInfo = string.Empty;
            var mc = new ManagementClass("win32_processor");
            var moc = mc.GetInstances();

            foreach (var o in moc)
            {
                var mo = o.As<ManagementObject>();
                cpuInfo = mo.Properties["processorID"].Value.ToString();
                break;
            }

            return cpuInfo;
        }
    }
}