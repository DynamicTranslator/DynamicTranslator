namespace DynamicTranslator.Core.ViewModel
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
            PropertyChanged += (sender, args) => OnPropertyChanged(sender, args.PropertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id
        {
            get { return id; }

            set
            {
                if (id == value)
                    return;

                id = value;
                PropertyChanged.InvokeSafely(this, new PropertyChangedEventArgs(nameof(Id)));
            }
        }

        public string ImageUrl
        {
            get { return imageUrl; }

            set
            {
                if (imageUrl == value)
                    return;

                imageUrl = value;
                PropertyChanged.InvokeSafely(this, new PropertyChangedEventArgs(nameof(ImageUrl)));
            }
        }

        public string Message
        {
            get { return message; }

            set
            {
                if (message == value)
                    return;

                message = value;
                PropertyChanged.InvokeSafely(this, new PropertyChangedEventArgs(nameof(Message)));
            }
        }

        public string Title
        {
            get { return title; }

            set
            {
                if (title == value)
                    return;

                title = value;

                PropertyChanged.InvokeSafely(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }

        protected virtual void OnPropertyChanged(object sender, string propertyName)
        {
        }
    }
}