using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UWP_Assignment.Service;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_Assignment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationPage : Page
    {
        public static Frame frame;
        public NavigationPage()
        {
            this.InitializeComponent();
            this.Loaded += NavigationPage_Loaded;
            frame = contentFrame;
        }

        private void NavigationPage_Loaded(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(Pages.ListAllSongPage));

        }
        private void nvSample_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            NavigationViewItemBase item = args.InvokedItemContainer;
            switch (item.Tag)
            {
                case "ListSong":
                    _ = contentFrame.Navigate(typeof(Pages.ListAllSongPage));
                    break;
                case "Information":
                    _ = contentFrame.Navigate(typeof(Pages.MyInformationPage));
                    break;
                case "MySong":
                    _ = contentFrame.Navigate(typeof(Pages.MySongPage));
                    break;
                case "CreateSong":
                    _ = contentFrame.Navigate(typeof(Pages.CreateSongPage));
                    break;
            }
        }

        private async void NavigationViewItem_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Đăng xuất",
                Content = "Bạn chắc chắn muốn đăng xuất",
                PrimaryButtonText = "Đăng xuất",
                CloseButtonText = "Hủy"
            };
            ContentDialogResult result = await deleteFileDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                    StorageFile manifestFile = await storageFolder.GetFileAsync(AccountService.userTokenFileName);
                    await manifestFile.DeleteAsync();
                    Frame rootFrame = Window.Current.Content as Frame;
                    rootFrame.Navigate(typeof(Pages.LoginPage));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Có lỗi xảy ra khi logout" + ex);
                }
            }
            else
            {
            }
        }
    }
}
