namespace DynamicTranslator.Core.Optimizers.Timing
{
    #region using

    using System;
    using Exception;

    #endregion

    public static class Clock
    {
        private static IClockProvider _provider;

        static Clock()
        {
            Provider = new LocalClockProvider();
        }

        /// <summary>
        ///     Gets Now using current <see cref="Provider" />.
        /// </summary>
        public static DateTime Now => Provider.Now;

        /// <summary>
        ///     This object is used to perform all <see cref="Clock" /> operations.
        ///     Default value: <see cref="LocalClockProvider" />.
        /// </summary>
        public static IClockProvider Provider
        {
            get { return _provider; }
            set
            {
                if (value == null)
                {
                    throw new BusinessException("Can not set Clock to null!");
                }

                _provider = value;
            }
        }

        /// <summary>
        ///     Normalizes given <see cref="DateTime" /> using current <see cref="Provider" />.
        /// </summary>
        /// <param name="dateTime">DateTime to be normalized.</param>
        /// <returns>Normalized DateTime</returns>
        public static DateTime Normalize(DateTime dateTime)
        {
            return Provider.Normalize(dateTime);
        }
    }
}