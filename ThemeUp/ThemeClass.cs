using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text; 
using System.Threading.Tasks;
using System.Windows.Controls; 

namespace ThemeUp
{
    public class ThemeClass
    {
        public GeoCoordinate geocoordinates
        {
            get;
            set;
        }

        private String id;
        public String mixId
        {
            get;
            set;
        }


        private String _name;
        public String name
        {
            get;
            set;
        }

        public String image {get;set;}
    }
}
