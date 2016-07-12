using System;
using System.Collections;
using System.Collections.Generic;

namespace DynamicTranslator
{
    [Serializable]
    public class Maybe<T> : IEnumerable<T>
    {
        public Maybe()
        {
            values = new T[0];
        }

        public Maybe(T value)
        {
            values = new[] {value};
        }

        private readonly IEnumerable<T> values;

        public IEnumerator<T> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}