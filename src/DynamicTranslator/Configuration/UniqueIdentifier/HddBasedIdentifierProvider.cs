using System.Diagnostics.CodeAnalysis;
using System.Management;

namespace DynamicTranslator.Configuration.UniqueIdentifier
{
    [SuppressMessage("ReSharper", "ComplexConditionExpression")]
    public class HddBasedIdentifierProvider : IUniqueIdentifierProvider
    {
        public string Get()
        {
            const string drive = "C";
            var dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
            dsk.Get();
            string volumeSerial = dsk["VolumeSerialNumber"].ToString();

            return volumeSerial;
        }
    }
}