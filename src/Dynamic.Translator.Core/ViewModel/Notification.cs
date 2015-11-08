namespace Dynamic.Translator.Core.ViewModel
{
    #region using

    using System.ComponentModel;
    using Dependency.Markers;
    using Extensions;
    using Interfaces;

    #endregion

    public class Notification : INotifyPropertyChanged, INotification, ITransientDependency
    {
        private int id;
        private string imageUrl;
        private string message;
        private string title;

        public Notification()
        {
            this.PropertyChanged += (sender, args) => this.OnPropertyChanged(sender, args.PropertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id
        {
            get { return this.id; }

            set
            {
                if (this.id == value)
                    return;

                this.id = value;
                this.PropertyChanged.InvokeSafely(this, new PropertyChangedEventArgs(nameof(this.Id)));
            }
        }

        public string ImageUrl
        {
            get { return this.imageUrl; }

            set
            {
                if (this.imageUrl == value)
                    return;

                this.imageUrl = value;
                this.PropertyChanged.InvokeSafely(this, new PropertyChangedEventArgs(nameof(this.ImageUrl)));
            }
        }

        public string Message
        {
            get { return this.message; }

            set
            {
                if (this.message == value)
                    return;

                this.message = value;
                this.PropertyChanged.InvokeSafely(this, new PropertyChangedEventArgs(nameof(this.Message)));
            }
        }

        public string Title
        {
            get { return this.title; }

            set
            {
                if (this.title == value)
                    return;

                this.title = value;

                this.PropertyChanged.InvokeSafely(this, new PropertyChangedEventArgs(nameof(this.Title)));
            }
        }

        protected virtual void OnPropertyChanged(object sender, string propertyName)
        {
        }
    }
}