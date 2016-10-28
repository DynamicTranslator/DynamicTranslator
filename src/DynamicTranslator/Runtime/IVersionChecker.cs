namespace DynamicTranslator.Runtime
{
    public interface IVersionChecker
    {
        bool IsNew(string incomingVersion);

        bool IsEqual(string version);
    }
}
