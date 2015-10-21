namespace Dynamic.Tureng.Translator.Orchestrator
{
    using System;

    public interface IMeanOrganizer
    {
        string BeginProcess(string text = null);

        event EventHandler<OnNotificationAddEventArgs> OnNotificationAdd;

    }
}