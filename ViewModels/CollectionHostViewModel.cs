using System.Collections.ObjectModel;
using System.ComponentModel;

namespace GymTrackerApp.ViewModels;

public abstract class CollectionHostViewModel<TItem> : ViewModelBase where TItem : INotifyPropertyChanged
{
        protected void AddWithEvents(
                ObservableCollection<TItem> collection,
                TItem item,
                PropertyChangedEventHandler handler)
        {
                collection.Add(item);
                item.PropertyChanged  += handler;
                
        }
        
        protected void RemoveWithEvents(
                ObservableCollection<TItem> collection,
                TItem item,
                PropertyChangedEventHandler handler)
        {
                item.PropertyChanged -= handler;
                collection.Remove(item);
        }

        protected void AttachExistingItems(
                ObservableCollection<TItem> collection,
                PropertyChangedEventHandler handler)
        {
                foreach (var item in collection)
                        item.PropertyChanged += handler;
                        
        }
}