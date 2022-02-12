using Lab_1.Utils;
using Newtonsoft.Json;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lab_1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private string checkedGender;
        private string checkedCalendarDatePicker;
        private int validateCheck;

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            checkedGender = rb.Content.ToString();
        }

        private void CalendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            CalendarDatePicker cdp = sender as CalendarDatePicker;
            checkedCalendarDatePicker = cdp.Date.Value.ToString("dd-MM-yyyy");
        }

        private bool ValidateForm(
            string firstName, string lastName, string password, string confirmPassword, 
            string address, string phone, string avatar, string email, string introduction, string checkedGender, string checkedCalendarDatePicker)
        {
            validateCheck = 0;
            if (Validator.IsEmpty(firstName))
            {
                msgFirstName.Text = "First name is required";
                validateCheck += 1;
            }
            else
            {
                msgFirstName.Text = "";
            }
            if (Validator.IsEmpty(lastName))
            {
                msgLastName.Text = "Last name is required";
                validateCheck += 1;
            }
            else
            {
                msgLastName.Text = "";
            }
            if (Validator.IsEmpty(phone))
            {
                msgPhone.Text = "Phone number is required";
                validateCheck += 1;
            }
            else
            {
                if (!Validator.IsPhoneNumber(phone))
                {
                    msgPhone.Text = "Phone number format is not correct";
                    validateCheck += 1;
                }
                else
                {
                    msgPhone.Text = "";
                }
            }
            if (Validator.IsEmpty(email))
            {
                msgEmail.Text = "Email is required";
                validateCheck += 1;
            }
            else
            {
                if (!Validator.IsEmail(email))
                {
                    msgEmail.Text = "Email format is not correct";
                    validateCheck += 1;
                }
                else
                {
                    msgPhone.Text = "";
                }

            }
            if (Validator.IsEmpty(avatar))
            {
                msgAvatar.Text = "Avatar is required";
                validateCheck += 1;
            }
            else
            {
                msgAvatar.Text = "";
            }
            if (Validator.IsEmpty(password))
            {
                msgPassword.Text = "Password is required";
                validateCheck += 1;
            }
            else
            {
                msgPassword.Text = "";
            }
            
            if (Validator.IsEmpty(confirmPassword))
            {
                msgConfirmPassword.Text = "Password confirm is required";
                validateCheck += 1;
            }
            else
            {
                if (!Validator.MatchString(password, confirmPassword))
                {
                    msgConfirmPassword.Text = "Password confirm is not match";
                    validateCheck += 1;
                }
                else
                {
                    msgConfirmPassword.Text = "";
                }
            }
            if (Validator.IsEmpty(address))
            {
                msgAddress.Text = "Address is required";
                validateCheck += 1;
            }
            else
            {
                msgAddress.Text = "";
            }
            if (Validator.IsEmpty(introduction))
            {
                msgIntroduction.Text = "Introduction is required";
                validateCheck += 1;
            }
            else
            {
                msgIntroduction.Text = "";
            }
            if (Validator.IsEmpty(checkedGender))
            {
                msgGender.Text = "Gender is required";
                validateCheck += 1;
            }
            else
            {
                msgGender.Text = "";
            }
            if (Validator.IsEmpty(checkedCalendarDatePicker))
            {
                msgDob.Text = "Birthday is required";
                validateCheck += 1;
            }
            else
            {
                msgDob.Text = "";
            }
            return validateCheck > 0;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm(txtFirstName.Text, txtLastName.Text, txtPassword.Password.ToString(), txtConfirmPassword.Password.ToString(),
                txtAddress.Text, txtPhone.Text, txtAvatar.Text, txtEmail.Text, txtIntroduction.Text, checkedGender, checkedCalendarDatePicker))
            {
                var form = new
                {
                    firsname = txtFirstName.Text,
                    lastname = txtLastName.Text,
                    password = txtPassword.Password.ToString(),
                    address = txtAddress.Text,
                    phone = txtPhone.Text,
                    introduction = txtIntroduction.Text,
                    email = txtEmail.Text,
                    gender = checkedGender,
                    dob = checkedCalendarDatePicker,
                
                };

                var jsonString = JsonConvert.SerializeObject(form);
                ContentDialog contentDialog = new ContentDialog();
                contentDialog.Title = "Register Information";
                contentDialog.Content = jsonString;
                contentDialog.CloseButtonText = "Close";
                await contentDialog.ShowAsync();
                return;
            }
            

        }

    }
}
