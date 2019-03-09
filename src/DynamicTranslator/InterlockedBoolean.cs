using System.Threading;

namespace DynamicTranslator
{
    /// <summary>
    /// Interlocked support for boolean values
    /// </summary>
    public class InterlockedBoolean
    {
        private int _value;

        /// <summary>
        /// Current value
        /// </summary>
        public bool Value
        {
            get { return _value == 1; }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="T:InterlockedBoolean"/>
        /// </summary>
        /// <param name="initialValue">initial value</param>
        public InterlockedBoolean(bool initialValue = false)
        {
            _value = initialValue ? 1 : 0;
        }

        /// <summary>
        /// Sets a new value
        /// </summary>
        /// <param name="newValue">new value</param>
        /// <returns>the original value before any operation was performed</returns>
        public bool Set(bool newValue)
        {
            var oldValue = Interlocked.Exchange(ref _value, newValue ? 1 : 0);
            return oldValue == 1;
        }

        /// <summary>
        /// Compares the current value and the comparand for equality and, if they are equal, 
        /// replaces the current value with the new value in an atomic/thread-safe operation.
        /// </summary>
        /// <param name="newValue">new value</param>
        /// <param name="comparand">value to compare the current value with</param>
        /// <returns>the original value before any operation was performed</returns>
        public bool CompareExchange(bool newValue, bool comparand)
        {
            var oldValue = Interlocked.CompareExchange(ref _value, newValue ? 1 : 0, comparand ? 1 : 0);
            return oldValue == 1;
        }
    }
}