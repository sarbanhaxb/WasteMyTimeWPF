using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WasteMyTime.Model
{
    public class BDOItem : INotifyPropertyChanged
    {

        private int _Id;
        private string _Number;
        private string _Title;
        private string _OriginManufacturing;
        private string _OriginProducts;
        private string _OriginProcess;
        private string _Compound;
        private string _CompoundPercentMin;
        private string _CompoundPercentMax;
        private string _CompoundNotice;
        private string _WasteNotice;
        private string _PhysicalState;
        private string _HazardClass;
        private string _AttributionCriteria;
        private string _Doc;

        public int Id
        {
            get => _Id;
            set 
            {
                _Id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Number
        {
            get => _Number; 
            set
            {
                _Number = value;
                OnPropertyChanged("Number");
            }
        }
        public string Title
        {
            get => _Title;
            set
            {
                _Title = value;
                OnPropertyChanged("Title");
            }
        }
        public string OriginManufacturing
        {
            get => _OriginManufacturing;
            set
            {
                _OriginManufacturing = value;
                OnPropertyChanged("OriginManufacturing");
            }
        }
        public string OriginProducts
        {
            get => _OriginProducts; 
            set
            {
                _OriginProducts = value;
                OnPropertyChanged("OriginProducts");
            }
        }
        public string OriginProcess
        {
            get => _OriginProcess;
            set
            {
                _OriginProcess = value;
                OnPropertyChanged("OriginProcess");
            }
        }
        public string Compound
        {
            get => _Compound;
            set
            {
                _Compound = value;
                OnPropertyChanged("Compound");
            }
        }
        public string CompoundPercentMin
        {
            get => _CompoundPercentMin;
            set
            {
                _CompoundPercentMin = value;
                OnPropertyChanged("CompoundPercentMin");
            }
        }
        public string CompoundPercentMax
        {
            get => _CompoundPercentMax;
            set
            {
                _CompoundPercentMax = value;
                OnPropertyChanged("CompoundPercentMax");
            }
        }
        public string CompoundNotice 
        {
            get => _CompoundNotice;
            set
            {
                _CompoundNotice = value;
                OnPropertyChanged("CompoundNotice");
            }
        }
        public string WasteNotice
        {
            get => _WasteNotice;
            set
            {
                _WasteNotice = value;
                OnPropertyChanged("WasteNotice");
            }
        }
        public string PhysicalState
        {
            get => _PhysicalState;
            set
            {
                _PhysicalState = value;
                OnPropertyChanged("PhysicalState");
            }
        }
        public string HazardClass
        {
            get => _HazardClass;
            set
            {
                _HazardClass = value;
                OnPropertyChanged("HazardClass");
            }
        }
        public string AttributionCriteria
        {
            get => _AttributionCriteria;
            set
            {
                _AttributionCriteria = value;
                OnPropertyChanged("AttributionCriteria");
            }
        }
        public string Doc
        {
            get => _Doc;
            set
            {
                _Doc = value;
                OnPropertyChanged("Doc");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
