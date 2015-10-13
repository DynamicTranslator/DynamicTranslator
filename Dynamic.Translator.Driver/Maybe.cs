namespace Dynamic.Translator.Driver
{
    #region using

    using System.Collections;
    using System.Collections.Generic;

    #endregion

    public class Maybe<T> : IEnumerable<T>
    {
        readonly private IEnumerable<T> values;

        public Maybe()
        {
            this.values = new T[0];
        }

        public Maybe(T value)
        {
            this.values = new[] {value};
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.values.GetEnumerator();
        }
    }

    public static class Maybe
    {
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return new Maybe<T>(value);
        }

        public static Maybe<T> Empty<T>()
        {
            return new Maybe<T>();
        }
    }
}