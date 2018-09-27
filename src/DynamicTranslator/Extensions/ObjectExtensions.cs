using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DynamicTranslator.Extensions
{
    public static class ObjectExtensions
    {
        public static T DeserializeAs<T>(this string @this) where T : class
        {
            return DeserializeFromStream<T>(new MemoryStream(Encoding.UTF8.GetBytes(@this)));
        }

        public static T DeserializeFromStream<T>(Stream jsonStream) where T : class
        {
            if (jsonStream == null)
            {
                throw new ArgumentNullException(nameof(jsonStream));
            }

            if (jsonStream.CanRead == false)
            {
                throw new ArgumentException("Json stream must support reading", nameof(jsonStream));
            }

            T deserializedObj;

            using (var sr = new StreamReader(jsonStream))
            {
                using (JsonReader reader = new JsonTextReader(sr))
                {
                    var serializer = new JsonSerializer();
                    deserializedObj = serializer.Deserialize<T>(reader);
                }
            }

            return deserializedObj;
        }

        public static T GetFirstValueInArrayGraph<T>(this JArray jarray)
        {
            return jarray.ForwardToken().Value<T>();
        }

        internal static JToken ForwardToken(this JToken token)
        {
            return token.HasValues ? token.First.ForwardToken() : token;
        }

        public static T Manipulate<T>(this T @this, Action<T> setAction)
        {
            setAction(@this);
            return @this;
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> act)
        {
            foreach (var item in list)
            {
                act(item);
            }
        }
    }
}
