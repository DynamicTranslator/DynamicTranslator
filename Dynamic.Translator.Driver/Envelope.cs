namespace Dynamic.Translator.Driver
{
    using System;

    public class Envelope
    {
        public Envelope(object body, string version)
            : this(body, version, CreateDefaultBodyTypeFor(body.GetType()))
        {
        }

        public Envelope(object body, string version, string bodyType)
        {
            this.Body = body;
            this.Version = version;
            this.BodyType = bodyType;
        }

        public object Body { get; }

        public string BodyType { get; }

        public string Version { get; }

        public static string CreateDefaultBodyTypeFor(Type type)
        {
            return type.Name.ToLowerInvariant();
        }
    }
}