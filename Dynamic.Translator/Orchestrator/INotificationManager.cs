namespace Dynamic.Tureng.Translator.Orchestrator
{
    using System;

    public interface INotificationManager
    {
        event EventHandler<OnNotificationAddEventArgs> OnNotificationAdd;
    }
}