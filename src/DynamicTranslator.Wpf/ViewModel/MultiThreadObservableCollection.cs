using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace DynamicTranslator.Wpf.ViewModel
{
    public class MultiThreadObservableCollection<T> : ObservableCollection<T>
    {
        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler collectionChanged = CollectionChanged;
            if (collectionChanged != null)
            {
                foreach (Delegate @delegate in collectionChanged.GetInvocationList())
                {
                    var nh = (NotifyCollectionChangedEventHandler)@delegate;
                    var dispatcherObject = nh.Target as DispatcherObject;
                    Dispatcher dispatcher = dispatcherObject?.Dispatcher;
                    if ((dispatcher != null) && !dispatcher.CheckAccess())
                    {
                        dispatcher.BeginInvoke(
                            (Action)(() => nh.Invoke(this,
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
