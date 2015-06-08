namespace Dynamic.Tureng.Translator.Model
{
    #region using

    using System.Collections.ObjectModel;
    using System.ComponentModel;

    #endregion

    public class Notification : INotifyPropertyChanged
    {
        private int id;
        private string imageUrl;
        private string message;
        private string title;

        public int Id
        {
            get { return id; }

            set
            {
                if (id == value)
                {
                    return;
                }

                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string ImageUrl
        {
            get { return imageUrl; }

            set
            {
                if (imageUrl == value)
                {
                    return;
                }

                imageUrl = value;
                OnPropertyChanged("ImageUrl");
            }
        }

        public string Message
        {
            get { return message; }

            set
            {
                if (message == value)
                {
                    return;
                }

                message = value;
                OnPropertyChanged("Message");
            }
        }

        public string Title
        {
            get { return title; }

            set
            {
                if (title == value)
                {
                    return;
                }

                title = value;
                OnPropertyChanged("Title");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class Notifications : ObservableCollection<Notification>
    {
    }
}
