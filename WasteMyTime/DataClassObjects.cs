using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteMyTime
{

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
}
