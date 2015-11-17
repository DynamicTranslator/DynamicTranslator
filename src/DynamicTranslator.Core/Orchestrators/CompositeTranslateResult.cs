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
        [DataMember]
        public string SearchText { get; set; }

        [DataMember]
        public ICollection<TranslateResult> Results { get; set; }

        [DataMember]
        public DateTime CreateDate { get; set; }
    }
}