namespace DynamicTranslator.Core.Configuration.UniqueIdentifier
{
    using System.Management;

#pragma warning disable CA1416 // Validate platform compatibility
    public class HddBasedIdentifierProvider : IUniqueIdentifierProvider
    {
        public string Get()
        {
            const string drive = "C";
            var dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
            dsk.Get();
            var volumeSerial = dsk["VolumeSerialNumber"].ToString();

            return volumeSerial;
        }
    }
}
#pragma warning restore CA1416 // Validate platform compatibility