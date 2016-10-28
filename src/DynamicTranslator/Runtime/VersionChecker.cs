using System;

using Abp.Dependency;

namespace DynamicTranslator.Runtime
{
    public class VersionChecker : IVersionChecker, ITransientDependency
    {
        public bool IsNew(string incomingVersion)
        {
            var currentVersion = new Version(ApplicationVersion.GetCurrentVersion());
            var newVersion = new Version(incomingVersion);

            return newVersion > currentVersion;
        }

        public bool IsEqual(string version)
        {
            var currentVersion = new Version(ApplicationVersion.GetCurrentVersion());
            var versionToCheck = new Version(version);

            return versionToCheck == currentVersion;
        }
    }
}
