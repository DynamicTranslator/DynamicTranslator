// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Notification.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the Notification type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Dynamic.Tureng.Translator
{
    #region Using

    using System.Collections.ObjectModel;
    using System.ComponentModel;

    #endregion

    public class Notification : INotifyPropertyChanged
    {
        private int _id;
        private string _imageUrl;
        private string _message;
        private string _title;

        public int Id
        {
            get { return _id; }

            set
            {
                if (_id == value)
                {
                    return;
                }

                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string ImageUrl
        {
            get { return _imageUrl; }

            set
            {
                if (_imageUrl == value)
                {
                    return;
                }

                _imageUrl = value;
                OnPropertyChanged("ImageUrl");
            }
        }

        public string Message
        {
            get { return _message; }

            set
            {
                if (_message == value)
                {
                    return;
                }

                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public string Title
        {
            get { return _title; }

            set
            {
                if (_title == value)
                {
                    return;
                }

                _title = value;
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