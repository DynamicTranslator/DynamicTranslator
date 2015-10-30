namespace Dynamic.Translator.Saga
{
    #region

    using System;
    using Automatonymous;

    #endregion

    public class Translator : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
    }
}
