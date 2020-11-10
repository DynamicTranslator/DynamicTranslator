using System.Management;
using DynamicTranslator.Core.Extensions;

namespace DynamicTranslator.Core.Configuration.UniqueIdentifier
{
    public class CpuBasedIdentifierProvider : IUniqueIdentifierProvider
    {
        public string Get()
        {
            var cpuInfo = string.Empty;
            var mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementBaseObject o in moc)
            {
                var mo = o.As<ManagementObject>();
                cpuInfo = mo.Properties["processorID"].Value.ToString();
                break;
            }

            return cpuInfo;
        }
    }
}