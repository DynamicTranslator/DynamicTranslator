namespace DynamicTranslator.Core.Configuration.UniqueIdentifier
{
    using System.Management;
    using Extensions;

#pragma warning disable CA1416 // Validate platform compatibility
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
#pragma warning restore CA1416 // Validate platform compatibility