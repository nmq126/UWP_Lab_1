using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_Assignment.Entity;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_Assignment.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyInformationPage : Page
    {
        public MyInformationPage()
        {
            this.InitializeComponent();
            this.Loaded += MyInformationPage_Loaded;
        }

        private void MyInformationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Account account = App.accountUser;
            txtName.Text = account.firstName + " " + account.lastName;
            txtEmail.Text = account.email;
            txtAddress.Text = account.address;
            txtPhone.Text = account.phone;
            BitmapImage bitmapImageAvatar = new BitmapImage(new Uri(account.avatar.ToString()));
            txtAvatar.ImageSource = bitmapImageAvatar;
            switch (account.gender)
            {
                case 1:
                    txtGender.Text = "Nam";
                    break;
                case 2:
                    txtGender.Text = "Nữ";
                    break;
                case 3:
                    txtGender.Text = "Khác";
                    break;
            }
            
            txtDob.Text = account.birthday.ToString();
            txtIntroduction.Text = account.introduction ?? "Chưa cập nhật";
        }
    }
}
