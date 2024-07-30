using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Cells;


namespace WasteMyTime
{
    public class BDOItems : ObservableCollection<BDOItem>
    {
        public BDOItems() { }
    }
    public class City : INotifyPropertyChanged
    {
        //БЫЛО
        //public int Id { get; set; }
        //public string Title { get; set; }
        //public ObservableCollection<ObjectItem> Objects { get; set; } = new ObservableCollection<ObjectItem>();
        public int Id { get; set; }
        public string Title { get; set; }
        public ObservableCollection<ObjectItem> Objects { get; set; }
        public bool isExpanded;


        public bool IsExpanded // Новое свойство
        {
            get => isExpanded;
            set { isExpanded = value; OnPropertyChanged(nameof(IsExpanded)); }
        }


        public City()
        {
            Objects = new ObservableCollection<ObjectItem>();
            isExpanded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public class ObjectItem : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int IdCity { get; set; }
        public string Title { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanging(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
    public class BDOItem
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public string OriginManufacturing { get; set; }
        public string OriginProducts { get; set; }
        public string OriginProcess { get; set; }
        public string Compound { get; set; }
        public double CompoundPercentMin { get; set; }
        public double CompoundPercentMax { get; set; }
        public string CompoundNotice { get; set; }
        public string WasteNotice { get; set; }
        public string PhysicalState { get; set; }
        public string HazardClass { get; set; }
        public string AttributionCriteria { get; set; }
        public string Doc { get; set; }

        public BDOItem() { }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Id;
                    case 1: return Number;
                    case 2: return Title;
                    case 3: return OriginManufacturing;
                    case 4: return OriginProducts;
                    case 5: return OriginProcess;
                    case 6: return Compound;
                    case 7: return CompoundPercentMin;
                    case 8: return CompoundPercentMax;
                    case 9: return CompoundNotice;
                    case 10: return WasteNotice;
                    case 11: return PhysicalState;
                    case 12: return HazardClass;
                    case 13: return AttributionCriteria;
                    case 14: return Doc;
                    default: throw new IndexOutOfRangeException("Индекс за пределами");
                }
            }
        }
    }
}
