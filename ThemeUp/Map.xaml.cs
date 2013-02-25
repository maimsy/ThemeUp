using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Controls.Maps.Platform;

namespace ThemeUp
{
    public partial class Map : PhoneApplicationPage
    { 
        public GeoCoordinateWatcher watcher;
        public double lat=98, lon=76;
        MapLayer Layer = new MapLayer();
        public int i = 0;
        public Pushpin pin;


        public Map()
        {
            InitializeComponent();
            map2.Mode = new AerialMode();
            
            //watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            //watcher.MovementThreshold = 20;
            //watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            //watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            //watcher.Start();

        }

        private void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    MessageBox.Show("Location Service is not enabled on the device");
                    break;

                case GeoPositionStatus.NoData:
                    MessageBox.Show(" The Location Service is working, but it cannot get location data.");
                    break;
            }
        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            //map2.Center = e.Position.Location;
            //map2.ZoomLevel = 15;
            //lat = e.Position.Location.Latitude;
            //lon = e.Position.Location.Longitude;

            //if (e.Position.Location.IsUnknown)
            //{
            //    MessageBox.Show("Please wait while your position is determined....");
            //    return;
            //}
        }

        

        private void appBarCenter_Click(object sender, EventArgs e)
        {
 
        }


        private void Map_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            

            Point clicklocation = e.GetPosition(this.map2);
            GeoCoordinate geo = new GeoCoordinate();
            geo = map2.ViewportPointToLocation(clicklocation);
            map2.ZoomLevel = 17;
            map2.Center = geo;
            pin = new Pushpin();
            pin.Location = geo;
            map2.Children.Add(pin);
            

            map2.Tap -= new EventHandler<System.Windows.Input.GestureEventArgs>(Map_Tap_1);

        }
        
        private void removepin_Click(object sender, EventArgs e)
        {
            map2.Children.Remove(pin);
            map2.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(Map_Tap_1);

        }

        private void done_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Add.xaml?lat="+ pin.Location.Latitude+"&lon="+pin.Location.Longitude , UriKind.Relative));
        }
    }
}