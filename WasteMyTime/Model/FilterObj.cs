using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WasteMyTime.Model
{
    public class FilterObj : INotifyPropertyChanged
    {

        private bool _IsChecked;
        private string _Title;

        public bool IsChecked
        {
            get => _IsChecked;
            set
            {
                _IsChecked = value;
                OnPropertyChanged("IsChecked");
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


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
