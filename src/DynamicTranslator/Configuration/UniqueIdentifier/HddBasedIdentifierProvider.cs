using System.Management;

namespace DynamicTranslator.Configuration.UniqueIdentifier
{
    public class HddBasedIdentifierProvider : IUniqueIdentifierProvider
    {
        public string Get()
        {
            const string Drive = "C";
            var dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + Drive + @":""");
            dsk.Get();
            var volumeSerial = dsk["VolumeSerialNumber"].ToString();

            return volumeSerial;
        }
    }
}