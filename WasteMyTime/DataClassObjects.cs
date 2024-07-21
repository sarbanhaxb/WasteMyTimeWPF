using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteMyTime
{

    public class City
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ObservableCollection<ObjectItem> Objects { get; set; } = new ObservableCollection<ObjectItem>();
    }

    public class ObjectItem
    {
        public int Id { get; set; }
        public int IdCity { get; set; }
        public string Title { get; set; }
    }
}
