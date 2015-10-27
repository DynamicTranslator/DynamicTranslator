namespace Dynamic.Translator.Core.ViewModel
{
    #region

    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Windows.Threading;

    #endregion

    public class MultiThreadObservableCollection<T> : ObservableCollection<T>
    {
        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler CollectionChanged = this.CollectionChanged;
            if (CollectionChanged != null)
            {
                foreach (var @delegate in CollectionChanged.GetInvocationList())
                {
                    var nh = (NotifyCollectionChangedEventHandler) @delegate;
                    DispatcherObject dispObj = nh.Target as DispatcherObject;
                    Dispatcher dispatcher = dispObj?.Dispatcher;
                    if (dispatcher != null && !dispatcher.CheckAccess())
                    {
                        dispatcher.BeginInvoke(
                            (Action) (() => nh.Invoke(this,
                                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))),
                            DispatcherPriority.DataBind);
                        continue;
                    }
                    nh.Invoke(this, e);
                }
            }
        }
    }
}
