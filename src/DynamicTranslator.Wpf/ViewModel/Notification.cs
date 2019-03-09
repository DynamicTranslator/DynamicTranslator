using System.ComponentModel;
using DynamicTranslator.Extensions;

namespace DynamicTranslator.Wpf.ViewModel
{
    public class Notification : INotifyPropertyChanged
    {
        private int _id;
        private string _imageUrl;
        private string _message;
        private string _title;

        public Notification()
        {
            PropertyChanged += (sender, args) => OnPropertyChanged(sender, args.PropertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id
        {
            get => _id;

            set
            {
                if (_id == value)
                    return;

                _id = value;
                PropertyChanged.InvokeSafely(this, new PropertyChangedEventArgs(nameof(Id)));
            }
        }

        public string ImageUrl
        {
            get => _imageUrl;

            set
            {
                if (_imageUrl == value)
                    return;

                _imageUrl = value;
                PropertyChanged.InvokeSafely(this, new PropertyChangedEventArgs(nameof(ImageUrl)));
            }
        }

        public string Message
        {
            get => _message;

            set
            {
                if (_message == value)
                    return;

                _message = value;
                PropertyChanged.InvokeSafely(this, new PropertyChangedEventArgs(nameof(Message)));
            }
        }

        public string Title
        {
            get => _title;

            set
            {
                if (_title == value)
                    return;

                _title = value;

                PropertyChanged.InvokeSafely(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }

        protected virtual void OnPropertyChanged(object sender, string propertyName) {}
    }
}