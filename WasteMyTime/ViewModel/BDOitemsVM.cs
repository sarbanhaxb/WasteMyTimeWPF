using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WasteMyTime.Model;

namespace WasteMyTime.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _filterText;
        private ICollectionView _bdoItemsView;

        public MainViewModel()
        {
            BDOItems = SQLquery.LoadBDO();

            BDOItemsView = CollectionViewSource.GetDefaultView(BDOItems);
            BDOItemsView.Filter = FilterBDOItems;
        }

        public ObservableCollection<BDOItem> BDOItems { get; set; }

        public ICollectionView BDOItemsView
        {
            get => _bdoItemsView;
            set
            {
                _bdoItemsView = value;
                OnPropertyChanged("BDOItemsView");
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged("FilterText");
                BDOItemsView.Refresh();
            }
        }

        private bool FilterBDOItems(object item)
        {
            if (item is BDOItem bdoItem)
            {
                return string.IsNullOrEmpty(FilterText)
                    || bdoItem.Number.ToLower().Contains(FilterText)
                    || bdoItem.Title.ToLower().Contains(FilterText)
                    || bdoItem.OriginManufacturing.ToLower().Contains(FilterText)
                    || bdoItem.OriginProducts.ToLower().Contains(FilterText)
                    || bdoItem.OriginProcess.ToLower().Contains(FilterText)
                    || bdoItem.Compound.ToLower().Contains(FilterText)
                    //|| bdoItem.CompoundPercentMin.Contains(FilterText)
                    //|| bdoItem.CompoundPercentMax.Contains(FilterText)
                    || bdoItem.CompoundNotice.ToLower().Contains(FilterText)
                    || bdoItem.WasteNotice.ToLower().Contains(FilterText)
                    || bdoItem.PhysicalState.ToLower().Contains(FilterText)
                    || bdoItem.HazardClass.ToLower().Contains(FilterText)
                    || bdoItem.AttributionCriteria.ToLower().Contains(FilterText)
                    || bdoItem.Doc.ToLower().Contains(FilterText);
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
