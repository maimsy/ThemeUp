using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Xml.Serialization;
using System.IO.IsolatedStorage;
using System.IO;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;

namespace ThemeUp
{
    public partial class MapThemes : PhoneApplicationPage
    {

        public ThemeClass theme;
        public List<ThemeClass> data;
        public string sFile = "storage.txt";
        public IsolatedStorageFile isf;
        public MapLayer Layer = new MapLayer();
        public GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
        ProgressIndicator progressIndicator = new ProgressIndicator() { IsVisible = true, IsIndeterminate = true, Text = "loading salons ..." };

        Pushpin pin1 = new Pushpin(); 
        public MapThemes()
        {
            InitializeComponent(); SystemTray.SetProgressIndicator(this, progressIndicator);



            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("Hello.xml", FileMode.OpenOrCreate))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<ThemeClass>));
                        data = (List<ThemeClass>)serializer.Deserialize(stream);
                        
                        foreach (ThemeClass d in data)
                        {
                            Pushpin pin = new Pushpin(); 
                            pin.Content = d.name;
                            map2.Children.Add(pin);
                            Layer.AddChild(pin, new GeoCoordinate(d.geocoordinates.Latitude,d.geocoordinates.Longitude));
                            }
                            map2.Children.Add(Layer); 
                        }
                    }
                }
            


            catch
            {
                //add some code here
            }
            progressIndicator.IsVisible = false;

            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            watcher.Start();
            watcher.MovementThreshold = 20; 
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
            map2.Center = e.Position.Location;  
            map2.ZoomLevel = 17;
           
            if (e.Position.Location.IsUnknown)
            {
                MessageBox.Show("Please wait while your position is determined....");
                return;
            }
        }

      
        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);
            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            watcher.Start();

        }
    }
}