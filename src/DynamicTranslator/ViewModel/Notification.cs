namespace DynamicTranslator.ViewModel
{
    using System.ComponentModel;
    using Core.Extensions;

    public class Notification : INotifyPropertyChanged
    {
        int id;
        string imageUrl;
        string message;
        string title;

        public Notification()
        {
            PropertyChanged += (sender, args) => OnPropertyChanged(sender, args.PropertyName);
        }

        public int Id
        {
            get => this.id;

            set
            {
                if (this.id == value)
                    return;

                this.id = value;
                PropertyChanged.InvokeSafely(this, new PropertyChangedEventArgs(nameof(Id)));
            }
        }

        public string ImageUrl
        {
            get => this.imageUrl;

            set
            {
                if (this.imageUrl == value)
                    return;

                this.imageUrl = value;
                PropertyChanged.InvokeSafely(this, new PropertyChangedEventArgs(nameof(ImageUrl)));
            }
        }

        public string Message
        {
            get => this.message;

            set
            {
                if (this.message == value)
                    return;

                this.message = value;
                PropertyChanged.InvokeSafely(this, new PropertyChangedEventArgs(nameof(Message)));
            }
        }

        public string Title
        {
            get => this.title;

            set
            {
                if (this.title == value)
                    return;

                this.title = value;

                PropertyChanged.InvokeSafely(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(object sender, string propertyName) { }
    }
}