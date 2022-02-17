using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UWP_Assignment.Entity;
using UWP_Assignment.Service;
using UWP_Assignment.Utils;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_Assignment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private int validateCheck;
        private AccountService accountService = new AccountService();
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void Login_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                return;
            }
            Credential result = await accountService.LoginAsync(txtEmail.Text, txtPassword.Password.ToString());
            ContentDialog contentDialog = new ContentDialog();
            if (result != null)
            {
                contentDialog.Title = "Đăng nhập";
                contentDialog.Content = "Đăng nhập thành công";
                Frame.Navigate(typeof(Pages.NavigationPage));
            }
            else
            {
                contentDialog.Title = "Đăng nhập";
                contentDialog.Content = "Đăng nhập thất bại!";
            }
            contentDialog.CloseButtonText = "Close";
            await contentDialog.ShowAsync();
        }   

        private bool ValidateForm()
        {
            validateCheck = 0;
            if (Validator.IsEmpty(txtEmail.Text))
            {
                msgEmail.Text = "*Vui lòng nhập email";
                validateCheck++;
            }
            else
            {
                msgEmail.Text = "";
            }
            if (Validator.IsEmpty(txtPassword.Password.ToString()))
            {
                msgPassword.Text = "*Vui lòng nhập mật khẩu";
                validateCheck++;
            }
            else
            {
                msgPassword.Text = "";
            }
            return validateCheck > 0;
        }

        private void Register_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pages.RegisterPage));
        }
    }
}
