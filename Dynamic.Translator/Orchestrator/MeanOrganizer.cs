namespace Dynamic.Tureng.Translator.Orchestrator
{
    using System;
    using System.Windows;
    using Dynamic.Translator.Core.Config;
    using Dynamic.Translator.Core.Extensions;
    using Dynamic.Translator.Core.ViewModel;
    using Observable;

    public class MeanOrganizer 
    {
        private readonly IStartupConfiguration _configurations;
        private string currentString;
        private string previousString;

        public MeanOrganizer(IStartupConfiguration configurations)
        {
            this._configurations = configurations;
        }

        public string BeginProcess(string text)
        {
            try
            {
               
            }
            catch (Exception)
            {
                //ingore
            }

            return null;
        }

        public event EventHandler<WhenNotificationAddEventArgs> OnNotificationAdd;
    }
}