namespace DynamicTranslator.Core.Orchestrators
{
    #region using

    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    #endregion

    [Serializable]
    public class CompositeTranslateResult
    {
        public CompositeTranslateResult(string searchText, int frequency, ICollection<TranslateResult> result, DateTime createDate)
        {
            Results = result;
            SearchText = searchText;
            CreateDate = createDate;
            Frequency = frequency;
        }

        [DataMember]
        public string SearchText { get; protected set; }

        [DataMember]
        public int Frequency { get; protected set; }

        [DataMember]
        public ICollection<TranslateResult> Results { get; protected set; }

        [DataMember]
        public DateTime CreateDate { get; protected set; }

        public CompositeTranslateResult SetResults(ICollection<TranslateResult> results)
        {
            Results = results;
            return this;
        }

        public CompositeTranslateResult SetCreateDate(DateTime datetime)
        {
            CreateDate = datetime;
            return this;
        }

        public CompositeTranslateResult IncreaseFrequency()
        {
            Frequency += 1;
            return this;
        }
    }
}