using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WasteMyTime
{
    public class Object
    {
        private int id;
        private int id_city;
        private string title;

        public int Id { get { return id; } set { id = value; } }

        public int Id_city { get { return id; } set { id_city = value;  } }

        public string Title { get { return title; } set { title = value; } }


        public Object() { }
        public Object(int id, int id_city, string title)
        {
            this.id = id;
            this.id_city = id_city;
            this.title = title;
        }
    }
}
