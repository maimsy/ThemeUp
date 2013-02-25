using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Nokia.Music.Phone;
using Nokia.Music.Phone.Types;
using System.IO.IsolatedStorage;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Device.Location;

namespace ThemeUp
{
    public partial class Add : PhoneApplicationPage
    {

        public List<ThemeClass> _list = new List<ThemeClass>();
        public ThemeClass theme= new ThemeClass();
        public ThemeClass theme1 = new ThemeClass();
     
        public IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        



        public Add()
        {
            InitializeComponent();
             

        }

        private void TextBlock_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Mix.xaml", UriKind.Relative));
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            base.OnNavigatedTo(e);
            string _artistId, _artistName,_image,_lat,_lon; 
           // MusicClientAsync client = new MusicClientAsync("uJka8T6ZDhdVDP-t1_zm", "9NdAMgcSdzWK1aczjfnQuA");
             

 
            if (NavigationContext.QueryString.TryGetValue("artistId", out _artistId))
            {
                if (NavigationContext.QueryString.TryGetValue("artistname", out _artistName))
                {
                    if (NavigationContext.QueryString.TryGetValue("image", out _image))
                    { 
                        theme.mixId = _artistId;
                        theme.name = _artistName;
                        settings.Add("name", theme.name);
                        settings.Add("id", theme.mixId);
                        settings.Add("image", _image);
                        //MessageBox.Show(_image);
                    }
                }
            }

           
            

                if (NavigationContext.QueryString.TryGetValue("lat", out _lat))
                {
                    if (NavigationContext.QueryString.TryGetValue("lon", out _lon))
                    {
                        //theme.geocoordinates.Latitude = Convert.ToDouble(_lat);
                        //theme.geocoordinates.Longitude = Convert.ToDouble(_lon);
                        settings.Add("lat",_lat);
                        settings.Add("lon",_lon);
                        double la = Convert.ToDouble(settings["lat"].ToString());
                        double lo = Convert.ToDouble(settings["lon"].ToString());
                        theme.geocoordinates = new GeoCoordinate(la, lo);
                    }
             
                }




                if (settings.Contains("name") == true && settings.Contains("lat")== true)
                {

                    try
                    {
                        using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                        {
                            using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("Hello.xml", FileMode.OpenOrCreate))
                            {
                                if (stream.Length > 0)
                                {
                                    XmlSerializer serializer = new XmlSerializer(typeof(List<ThemeClass>));
                                    _list = (List<ThemeClass>)serializer.Deserialize(stream);
                                }
                                
                            }
                        }
                    }
                    catch
                    {
                        //MessageBox.Show("nothing in the list");  
                    }

                    theme1.name = settings["name"].ToString();
                    theme1.mixId = settings["id"].ToString();
                    theme1.image =  settings["image"].ToString(); 
                    double la = Convert.ToDouble(settings["lat"].ToString());
                    double lo = Convert.ToDouble(settings["lon"].ToString());
                    theme1.geocoordinates = new GeoCoordinate(la, lo);

                    _list.Add(theme1);
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                    xmlWriterSettings.Indent = true;
                    using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                    {

                        using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("Hello.xml", FileMode.OpenOrCreate))
                        {

                            XmlSerializer serializer = new XmlSerializer(typeof(List<ThemeClass>));
                            using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                            {
                                serializer.Serialize(xmlWriter, _list);
                                 this.DataContext = theme1;
                            }
                             
                        }
                    } 
                } 
             
               else this.DataContext = theme;

        }



         
        private void TextBlock_Tap_2(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Map.xaml", UriKind.Relative));
        }

        private void done_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }
    }
}