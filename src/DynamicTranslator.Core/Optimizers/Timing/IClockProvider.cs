namespace DynamicTranslator.Core.Optimizers.Timing
{
    #region using

    using System;

    #endregion

    public interface IClockProvider
    {
        /// <summary>
        ///     Gets Now.
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        ///     Normalizes given <see cref="DateTime" />.
        /// </summary>
        /// <param name="dateTime">DateTime to be normalized.</param>
        /// <returns>Normalized DateTime</returns>
        DateTime Normalize(DateTime dateTime);
    }
}