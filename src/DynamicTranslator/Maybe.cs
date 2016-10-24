using System;
using System.Collections;
using System.Collections.Generic;

namespace DynamicTranslator
{
    [Serializable]
    public class Maybe<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _values;

        public Maybe()
        {
            _values = new T[0];
        }

        public Maybe(T value)
        {
            _values = new[] { value };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _values.GetEnumerator();
        }
    }
}
