using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using UWP_Assignment.Entity;
using UWP_Assignment.Service;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_Assignment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MySongPage : Page
    {
        private SongService songService;
        private MediaPlaybackList mediaPlaybackList;
        public MySongPage()
        {
            this.InitializeComponent();
            songService = new SongService();
            this.Loaded += MySongPage_Loaded;     

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            MyMediaPlayer.MediaPlayer.Pause();
            base.OnNavigatingFrom(e);
        }
        private async void MySongPage_Loaded(object sender, RoutedEventArgs e)
        {
            List<Song> listSong = await songService.GetMySongAsync();
            loadingGif.Visibility = Visibility.Collapsed;
            if (listSong.Count == 0)
            {
                noSong.Visibility = Visibility.Visible;
                return;
            }
            ObservableCollection<Song> observableSongs = new ObservableCollection<Song>(listSong);
            MyListSong.ItemsSource = observableSongs;

            mediaPlaybackList = new MediaPlaybackList();
            foreach (var song in listSong)
            {
                try
                {
                    var mediaPlaybackItem = new MediaPlaybackItem(MediaSource.CreateFromUri(new Uri(song.link)));
                    mediaPlaybackList.Items.Add(mediaPlaybackItem);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }

            }
            var mediaPlayer = new MediaPlayer();
            mediaPlayer.Source = mediaPlaybackList; // MediaPlayerElement < MediaPlayer < MediaPlaybackList < MediaPlaybackItem           
            MyMediaPlayer.SetMediaPlayer(mediaPlayer);
        }
        private void MyListSong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Song currentSong = MyListSong.SelectedItem as Song;
            txtCurrentSongName.Text = currentSong.name;
            txtCurrentSingerName.Text = currentSong.singer;
            BitmapImage thumbnail = new BitmapImage(new Uri(currentSong.thumbnail));
            txtThumbnail.ImageSource = thumbnail;
            AnimateProgressRingSlice();
            for (var i = 0; i < mediaPlaybackList.Items.Count; i++)
            {
                MediaPlaybackItem item = mediaPlaybackList.Items[i];
                if (string.Equals(currentSong.link, item.Source.Uri.ToString()))
                {
                    mediaPlaybackList.MoveTo(Convert.ToUInt32(i));
                }
                else
                { }
            }
        }
        private void AnimateProgressRingSlice()
        {
            TheImage.Visibility = Visibility.Visible;
            var storyboard = new Storyboard();
            var doubleAnimation = new DoubleAnimation();
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = TimeSpan.FromMilliseconds(3000);
            doubleAnimation.EnableDependentAnimation = true;
            doubleAnimation.To = 360;
            Storyboard.SetTargetProperty(doubleAnimation, "Angle");
            Storyboard.SetTarget(doubleAnimation, ImageRotation);
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }

        private void TextBlock_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pages.CreateSongPage));
        }
    }
}
