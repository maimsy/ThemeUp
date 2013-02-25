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
using Nokia.Music.Phone.Tasks;
using System.Runtime.Serialization.Json;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Device.Location;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Resources;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Nokia.Music.Phone;
using Windows.System;
using System.Xml;
using System.Xml.Serialization;
using Windows.Devices.Geolocation;
using Nokia.Music.Phone.Types;
using Microsoft.Devices;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;


//USE ExistToVisibilityConverter

namespace ThemeUp
{
    public partial class MainPage : PhoneApplicationPage
    {
        public ThemeClass theme;
        public List<ThemeClass> data;
        public string sFile = "storage.txt";
        public IsolatedStorageFile isf;
        public string weather = "http://free.worldweatheronline.com/feed/weather.ashx?key=xxx&q=yyy,zzz&num_of_days=3&format=xml";
        


        private void RenderString(String bitmap, string stringToRender)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = stringToRender;

            Rectangle rect = new Rectangle();
            rect.Fill = new SolidColorBrush(Colors.Red);

            rect.Width = 100;
            rect.Height = 100;

            BitmapImage img = new BitmapImage(new Uri(bitmap, UriKind.Relative));
            img.CreateOptions = BitmapCreateOptions.None;

             img.ImageOpened += (s, e) =>
            {
                WriteableBitmap wbm = new WriteableBitmap((BitmapImage)s); 
                wbm.Render(rect, null);
                wbm.Invalidate();
                
            };

            
        }

        public CameraCaptureTask cameraCaptureTask = new CameraCaptureTask();
           
        public MainPage()
        {

            InitializeComponent();

           
            cameraCaptureTask.Completed += cameraCaptureTask_Completed;

           // currenImg.Source = new BitmapImage(new Uri("themePH.jpg",UriKind.Relative));
            
            //Launcher.LaunchUriAsync(new Uri("nokia-music://search/anything/?term=baba")); 

            //this.Loaded += new RoutedEventHandler(MainPage_Loaded); 

           

            try
            {
                using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (IsolatedStorageFileStream stream = myIsolatedStorage.OpenFile("Hello.xml", FileMode.OpenOrCreate))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<ThemeClass>));
                        data = (List<ThemeClass>)serializer.Deserialize(stream);
                        listseletor.ItemsSource = data;
                         

                        if (data.Count > 0)
                        {

                            emptyStack.Visibility = Visibility.Collapsed;
                            currentStack.Visibility = Visibility.Visible;
                            currentTxtBox.Text = data[0].name;
                        }
                        


                    }
                }
            }
            catch
            {
                //add some code here
            }



            ShellTile PrimaryTile = ShellTile.ActiveTiles.First();

            if (PrimaryTile != null)
            {
                StandardTileData tile = new StandardTileData();


                tile.BackBackgroundImage = new Uri("themePH.jpg", UriKind.Relative);
              
                tile.BackgroundImage = new Uri("ApplicationIcon.jpg", UriKind.Relative);
                tile.BackTitle=data[0].name; 
                PrimaryTile.Update(tile);
            } 

            //WebClient client = new WebClient();
            //client.OpenReadCompleted += new OpenReadCompletedEventHandler(client_OpenReadCompleted);
            //client.OpenReadAsync(new Uri( data[2].image) );  

        }

        private void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if(e.TaskResult == TaskResult.OK)
           {
               ShowShareMediaTask(e.OriginalFileName);
           }
       }


       void ShowShareMediaTask(string path)
       {
           ShareMediaTask shareMediaTask = new ShareMediaTask();
           shareMediaTask.FilePath = path;
           shareMediaTask.Show();
       }
         


            void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
            {
                IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication();

                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream("image.jpg", System.IO.FileMode.Create, file))
                {
                    byte[] buffer = new byte[1024];
                    while (e.Result.Read(buffer, 0, buffer.Length) > 0)
                    {
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }

                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream("image.jpg", System.IO.FileMode.Open, file))
                {
                    BitmapImage image = new BitmapImage();
                    image.SetSource(stream);

                    currenImg.Source = image;
                }
            }
         
        private void mapTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PivotPage.xaml", UriKind.Relative));
        }

        private void mapTap(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PivotPage.xaml", UriKind.Relative));
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Add.xaml", UriKind.Relative));
     
        }

        private void LatitudeTextBlock_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Page2.xaml", UriKind.Relative));
        }

        void getdist( double n, double l)
        {
            foreach (ThemeClass d in data)
            {
                GeoCoordinate geo1 = new GeoCoordinate(d.geocoordinates.Latitude, d.geocoordinates.Longitude);
                GeoCoordinate geo2 = new GeoCoordinate(n,l );
                double distance = geo1.GetDistanceTo(geo2); 
                if (distance < 10)
                { 
                    //update tile mn
                    currentTxtBox.Text = d.name;

                    Uri uri = new Uri(d.image, UriKind.Absolute);
                    ImageSource imgSource = new BitmapImage(uri);
                    currenImg.Source = imgSource;

                    MessageBox.Show("less than 10");
                }
                else
                {  //empty tile mn

                }

            }
        }

        void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {

            double lat = args.Position.Coordinate.Latitude, lon = args.Position.Coordinate.Longitude;
            getdist( lat, lon); 
            

            if (!App.RunningInBackground)
            {

                Dispatcher.BeginInvoke(() =>
                {
                     

                    //LatitudeTextBlock.Content = args.Position.Coordinate.Latitude.ToString("0.00");
                    //LongitudeTextBlock.Text = args.Position.Coordinate.Longitude.ToString("0.00");
                    
                });
            }
            else
            {
               

                //Microsoft.Phone.Shell.ShellToast toast = new Microsoft.Phone.Shell.ShellToast();
                //toast.Content = args.Position.Coordinate.Latitude.ToString("0.00");
                //toast.Title = "Location: ";
                //toast.NavigationUri = new Uri("/Page2.xaml", UriKind.Relative);
                 
            } 
        }

        private void mn()
        {  
            try
            {
                new PlayMixTask()
                {
                    ArtistName = data[0].name
                }.Show();
            }
            catch { }
        }



        protected override  void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        { 

            if (App.Geolocator == null)
            {
                App.Geolocator = new Geolocator();
                App.Geolocator.DesiredAccuracy = PositionAccuracy.High;
                App.Geolocator.MovementThreshold = 10; // The units are meters.
                
             App.Geolocator.PositionChanged += geolocator_PositionChanged;
            }
        }

         

        private void mapthemeTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MapThemes.xaml", UriKind.Relative));
        }

        private void takepic_click(object sender, EventArgs e)
        {
            
                       cameraCaptureTask.Show();

        }

        private void photoCaptureOrSelectionCompleted(object sender, PhotoResult e)
        { 
            if (e.TaskResult == TaskResult.OK)
            {

                //MessageBox.Show(e.ChosenPhoto.Length.ToString());  //Code to display the photo on the page in an image control named myImage. 


             }


            else
            {
                MessageBox.Show("something has gone awry.");
            }
        }

        private void about_click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }


        private void settings_click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }

        private void currentthemeclick(object sender, System.Windows.Input.GestureEventArgs e)
        {
            mn();
        }
         
    }
     }
