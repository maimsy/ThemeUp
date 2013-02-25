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

namespace ThemeUp
{
    public partial class list : PhoneApplicationPage
    {
        public list()
        {
            InitializeComponent();

        }


        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                MusicClientAsync client = new MusicClientAsync("uJka8T6ZDhdVDP-t1_zm", "9NdAMgcSdzWK1aczjfnQuA");
                var artists = await client.GetTopArtistsForGenre(NavigationContext.QueryString["id"],itemsPerPage:30);
              
                
                //   var artists = await client.GetMixes(NavigationContext.QueryString["id"], itemsPerPage: 30);
                if (artists.Error == null)
                {

                    ResultsListBox.ItemsSource = artists.Result;
                }

                else MessageBox.Show(Convert.ToString(artists.Error));

            }
        }

        private void ShowItem(object sender, SelectionChangedEventArgs e)
        {
            //lonlist selector/multiselecter
            //appbar play
            var artists = ResultsListBox.SelectedItem as Artist;
            string m = Convert.ToString(artists.Thumb100Uri); 
            //NavigationService.Navigate(new Uri("/Add.xaml?artistId=" + artists.Id + "&" + "image=" + m + "&artistname=" + artists.Name, UriKind.Relative));
            
            artists.PlayMix();
           

        }
    }
}