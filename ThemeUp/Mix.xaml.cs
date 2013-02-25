using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Windows.System;
using Nokia.Music.Phone;
using Nokia.Music.Phone.Types;

namespace ThemeUp
{
    public partial class Mix : PhoneApplicationPage
    {
        public Mix()
        {
            InitializeComponent(); 

        }



        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                MusicClientAsync client = new MusicClientAsync("uJka8T6ZDhdVDP-t1_zm", "9NdAMgcSdzWK1aczjfnQuA");
                var genres = await client.GetGenres();
                if (genres.Error == null)
                { 
                    ResultsListBox.ItemsSource = genres.Result;
                }

                else MessageBox.Show(Convert.ToString(genres.Error));

            }
        }

        private void ShowItem(object sender, SelectionChangedEventArgs e)
        {
            var genre = ResultsListBox.SelectedItem as Genre;
            if (genre != null)
            {
                NavigationService.Navigate(new Uri("/list.xaml?id=" + genre.Id, UriKind.Relative));


            }




        }
    }
}
