using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteMyTime
{
    public class City
    {
        private int id;
        private string title;

        public string Title {  get { return title; } set { title = value; } }

        public int Id { get { return id; } set { id = value; } }

        public City() { }
        public City(int id, string title)
        {
            this.id = id;
            this.title = title;
        }
    }
}
