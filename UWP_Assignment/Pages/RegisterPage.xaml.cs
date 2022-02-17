using System.Threading.Tasks;
using UWP_Assignment.Entity;
using UWP_Assignment.Utils;
using UWP_Assignment.Service;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;
using Windows.UI.Xaml.Input;
using System.Diagnostics;
using CloudinaryDotNet;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using CloudinaryDotNet.Actions;
using System.IO;
using System.Collections.Generic;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_Assignment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Windows.UI.Xaml.Controls.Page
    {
        private int checkedGender;
        private int validateCheck;
        private string checkedDob;
        private AccountService accountService = new AccountService();
        private static string idCloudinary;
        private Cloudinary cloudinary;


        public RegisterPage()
        {
            this.InitializeComponent();
            this.Loaded += RegisterPage_Loaded;
        }

        private void RegisterPage_Loaded(object sender, RoutedEventArgs e)
        {
           CloudinaryDotNet.Account accountCloudinary = new CloudinaryDotNet.Account(
           "nmqdec6",
           "627924869682866",
           "txZANZ31hAKY603Pr7J9O43Ys3s"
);
            cloudinary = new Cloudinary(accountCloudinary);
            cloudinary.Api.Secure = true;
        }

        private async void btnUpload_Click(object sender, RoutedEventArgs e)
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
                BitmapImage bitmapImageAvatar = new BitmapImage();
                IRandomAccessStream fileStream = await file.OpenReadAsync();
                await bitmapImageAvatar.SetSourceAsync(fileStream);
                imgAvatar.Source = bitmapImageAvatar;
                RawUploadParams imageUploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.Name, await file.OpenStreamForReadAsync())
                };                RawUploadResult result = await cloudinary.UploadAsync(imageUploadParams);
                idCloudinary = result.PublicId;
                txtAvatar.Text = result.Url.ToString();
                btnUpload.Visibility = Visibility.Collapsed;
                btnDelete.Visibility = Visibility.Visible;
            }
            else
            {
                Debug.WriteLine("Upload fail");
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            List<string> listPublicIdCouldinary = new List<string>();
            listPublicIdCouldinary.Add(idCloudinary);
            string[] listIdCloudinary = listPublicIdCouldinary.ToArray();
            await cloudinary.DeleteResourcesAsync(listIdCloudinary);
            btnDelete.Visibility = Visibility.Collapsed;
            btnUpload.Visibility = Visibility.Visible;
            imgAvatar.Source = null;
            txtAvatar.Text = "";
        }

        private async void Register_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                return;
            }
            Entity.Account account = new Entity.Account()
            {
                firstName = txtFirstName.Text,
                lastName = txtLastName.Text,
                email = txtEmail.Text,
                phone = txtPhone.Text,
                password = txtPassword.Password.ToString(),
                address = txtAddress.Text,
                gender = checkedGender,
                avatar = txtAvatar.Text,
                birthday = checkedDob,
                introduction = txtIntroduction.Text,
            };
            var result = await accountService.RegisterAsync(account);
            ContentDialog contentDialog = new ContentDialog();
            if (result)
            {
                contentDialog.Title = "Đăng ký";
                contentDialog.Content = "Đăng ký tài khoản thành công!";
                Frame.Navigate(typeof(Pages.LoginPage));
            }
            else
            {
                contentDialog.Title = "Đăng ký";
                contentDialog.Content = "Đăng ký tài khoản thất bại!";
            }
            contentDialog.CloseButtonText = "Oke";
            await contentDialog.ShowAsync();
        }
        private bool ValidateForm()
        {
            validateCheck = 0;
            if (Validator.IsEmpty(txtFirstName.Text))
            {
                msgFirstName.Text = "*Vui lòng điền họ";
                validateCheck += 1;
            }
            else
            {
                msgFirstName.Text = "";
            }
            if (Validator.IsEmpty(txtLastName.Text))
            {
                msgLastName.Text = "*Vui lòng điền tên";
                validateCheck += 1;
            }
            else
            {
                msgLastName.Text = "";
            }
            if (Validator.IsEmpty(txtPhone.Text))
            {
                msgPhone.Text = "*Vui lòng nhập số điện thoại";
                validateCheck += 1;
            }
            else
            {
                if (!Validator.IsPhoneNumber(txtPhone.Text))
                {
                    msgPhone.Text = "*Số điện thoại không đúng định dạng";
                    validateCheck += 1;
                }
                else
                {
                    msgPhone.Text = "";
                }
            }
            if (Validator.IsEmpty(txtEmail.Text))
            {
                msgEmail.Text = "*Vui lòng nhập email";
                validateCheck += 1;
            }
            else
            {
                if (!Validator.IsEmail(txtEmail.Text))
                {
                    msgEmail.Text = "*Email không đúng định dạng";
                    validateCheck += 1;
                }
                else
                {
                    msgEmail.Text = "";
                }

            }
            if (Validator.IsEmpty(txtAvatar.Text))
            {
                msgAvatar.Text = "Vui lòng tải lên ảnh đại diện";
                validateCheck += 1;
            }
            else
            {
                msgAvatar.Text = "";
            }
            if (Validator.IsEmpty(txtPassword.Password.ToString()))
            {
                msgPassword.Text = "*Vui lòng nhập mật khẩu";
                validateCheck += 1;
            }
            else
            {
                msgPassword.Text = "";
            }

            if (Validator.IsEmpty(txtConfirmPassword.Password.ToString()))
            {
                msgConfirmPassword.Text = "*Vui lòng xác nhận mật khẩu";
                validateCheck += 1;
            }
            else
            {
                if (!Validator.MatchString(txtPassword.Password.ToString(), txtConfirmPassword.Password.ToString()))
                {
                    msgConfirmPassword.Text = "*Mật khẩu xác nhận không đúng";
                    validateCheck += 1;
                }
                else
                {
                    msgConfirmPassword.Text = "";
                }
            }
            if (Validator.IsEmpty(txtAddress.Text))
            {
                msgAddress.Text = "*Vui lòng nhập địa chỉ";
                validateCheck += 1;
            }
            else
            {
                msgAddress.Text = "";
            }
            if (checkedGender == 0)
            {
                msgGender.Text = "*Vui lòng lựa chọn giới tính";
                validateCheck += 1;
            }
            else
            {
                msgGender.Text = "";

            }
            if (Validator.IsEmpty(checkedDob))
            {
                msgDob.Text = "*Vui lòng nhập ngày sinh";
                validateCheck += 1;
            }
            else
            {
                msgDob.Text = "";
            }
            return validateCheck > 0;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var check = sender as RadioButton;
            switch (check.Content)
            {
                case "Nam":
                    checkedGender = 1;
                    break;
                case "Nữ":
                    checkedGender = 2;
                    break;
                case "Khác":
                    checkedGender = 3;
                    break;
            }
        }
        private void Dob_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            var date = sender;
            checkedDob = date.Date.Value.ToString("yyyy-MM-dd");
        }
        
        private void Login_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pages.LoginPage));
        }

    }
}
