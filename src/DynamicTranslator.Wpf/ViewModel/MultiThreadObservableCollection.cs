using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace DynamicTranslator.ViewModel
{
    public class MultiThreadObservableCollection<T> : ObservableCollection<T>
    {
        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var collectionChanged = CollectionChanged;
            if (collectionChanged == null) return;

            foreach (Delegate @delegate in collectionChanged.GetInvocationList())
            {
                var nh = (NotifyCollectionChangedEventHandler)@delegate;
                var dispatcherObject = nh.Target as DispatcherObject;
                Dispatcher dispatcher = dispatcherObject?.Dispatcher;
                if (dispatcher != null && !dispatcher.CheckAccess())
                {
                    dispatcher.BeginInvoke(priority:DispatcherPriority.DataBind,
                        (Action)(() => nh.Invoke(this,
                            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset))));
                    continue;
                }

                nh.Invoke(this, e);
            }
        }
    }
}
