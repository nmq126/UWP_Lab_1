using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UWP_Assignment.Entity;
using UWP_Assignment.Utils;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_Assignment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateSongPage : Windows.UI.Xaml.Controls.Page
    {
        private static string idCloudinary;
        private Cloudinary cloudinary;
        private int validateCheck;

        public CreateSongPage()
        {
            this.InitializeComponent();
            this.Loaded += CreateSongPage_Loaded;
        }

        private void CreateSongPage_Loaded(object sender, RoutedEventArgs e)
        {
            CloudinaryDotNet.Account accountCloudinary = new CloudinaryDotNet.Account(
            "nmqdec6",
            "627924869682866",
            "txZANZ31hAKY603Pr7J9O43Ys3s"
            );
            cloudinary = new Cloudinary(accountCloudinary);
            cloudinary.Api.Secure = true;
        }
        private bool ValidateForm()
        {
            validateCheck = 0;
            if (Validator.IsEmpty(txtName.Text))
            {
                msgName.Text = "*Vui lòng nhập tên bài hát";
                validateCheck += 1;
            }
            else
            {
                msgName.Text = "";
            }
            if (Validator.IsEmpty(txtSinger.Text))
            {
                msgSinger.Text = "*Vui lòng nhập tên ca sĩ";
                validateCheck += 1;
            }
            else
            {
                msgSinger.Text = "";
            }
            if (Validator.IsEmpty(txtAuthor.Text))
            {
                msgAuthor.Text = "*Vui lòng nhập tác giả";
                validateCheck += 1;
            }
            else
            {
                msgAuthor.Text = "";

            }
            if (Validator.IsEmpty(txtDescription.Text))
            {
                msgDescription.Text = "*Vui lòng nhập mô tả";
                validateCheck += 1;
            }
            else
            {
                msgDescription.Text = "";
            }
            if (Validator.IsEmpty(txtThumbnail.Text))
            {
                msgThumbnail.Text = "*Vui lòng chọn ảnh";
                validateCheck += 1;
            }
            else
            {
                msgThumbnail.Text = "";
            }

            if (Validator.IsEmpty(txtLink.Text))
            {
                msgLink.Text = "*Vui lòng nhập link bài hát";
                validateCheck += 1;
            }
            else
            {
                if (!Validator.IsValidSongLink(txtLink.Text))
                {
                    msgLink.Text = "*Link bài hát cần ở định dạng .mp3";
                    validateCheck += 1;
                }
                else
                {
                    msgLink.Text = "";
                }
            }

            return validateCheck > 0;
        }
        private async void Button_CreateSong(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                return;
            }
            loadingGif.Visibility = Visibility.Visible;
            var song = new Song()
            {
                name = txtName.Text,
                description = txtDescription.Text,
                singer = txtSinger.Text,
                author = txtAuthor.Text,
                thumbnail = txtThumbnail.Text,
                link = txtLink.Text,
            };
            bool result = await Service.SongService.CreateSongAsync(song);
            loadingGif.Visibility = Visibility.Collapsed;
            ContentDialog contentDialog = new ContentDialog();
            if (result)
            {
                contentDialog.Title = "Thành công";
                contentDialog.Content = "Bài hát đã được tạo thành công.";
                ClearForm();
            }
            else
            {
                contentDialog.Title = "Thất bại";
                contentDialog.Content = "Có lỗi xảy ra khi tạo bài hát.";
            }
            contentDialog.CloseButtonText = "Đóng";
            await contentDialog.ShowAsync();
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtAuthor.Text = "";
            txtDescription.Text = "";
            txtSinger.Text = "";
            txtThumbnail.Text = "";
            txtLink.Text = "";
            imgThumbnail.Source = null;
        }

        private async void Button_Upload(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                BitmapImage bitmapImage = new BitmapImage();
                IRandomAccessStream fileStream = await file.OpenReadAsync();
                await bitmapImage.SetSourceAsync(fileStream);
                imgThumbnail.Source = bitmapImage;
                RawUploadParams imageUploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.Name, await file.OpenStreamForReadAsync())
                };
                RawUploadResult result = await cloudinary.UploadAsync(imageUploadParams);
                idCloudinary = result.PublicId;
                txtThumbnail.Text = result.Url.ToString();
                btnUpload.Visibility = Visibility.Collapsed;
                btnDelete.Visibility = Visibility.Visible;
            }
            else
            {
                Debug.WriteLine("Create song upload fail");
            }
        }

        private async void Button_Delete(object sender, RoutedEventArgs e)
        {
            List<string> listPublicIdCouldinary = new List<string>();
            listPublicIdCouldinary.Add(idCloudinary);
            string[] listIdCloudinary = listPublicIdCouldinary.ToArray();
            await cloudinary.DeleteResourcesAsync(listIdCloudinary);
            btnDelete.Visibility = Visibility.Collapsed;
            btnUpload.Visibility = Visibility.Visible;
            imgThumbnail.Source = null;
            txtThumbnail.Text = "";
        }
    }
}
