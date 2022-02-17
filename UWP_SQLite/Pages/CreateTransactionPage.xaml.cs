using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_SQLite.Entity;
using UWP_SQLite.Utils;
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

namespace UWP_SQLite.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateTransactionPage : Page
    {
        private DateTime date;
        private int validateCheck;
        public CreateTransactionPage()
        {
            this.InitializeComponent();
            this.Loaded += CreateTransactionPage_Loaded;
        }

        private void CreateTransactionPage_Loaded(object sender, RoutedEventArgs e)
        {
            List<Category> listCategory = Data.DatabaseInitialize.ShowListCategory();
            listViewCategory.ItemsSource = listCategory;
        }

        private async void Submit(object sender, RoutedEventArgs e)
        {
            if(!ValidateForm())
            {
                PersonalTransaction transaction = new PersonalTransaction
                {
                    Name = txtName.Text,
                    Description = txtDescription.Text,
                    Detail = txtDetail.Text,
                    Amount = Double.Parse(txtAmount.Text),
                    Category = int.Parse(category.Tag.ToString()),
                    CreatedAt = Date.Date.Value.DateTime
                };
                ContentDialog contentDialog = new ContentDialog();
                if (Data.DatabaseInitialize.InsertData(transaction))
                {
                    contentDialog.Title = "Action success";
                    contentDialog.Content = "Added transaction successfully";
                    ClearForm();
                }
                else{
                    contentDialog.Title = "Action fail";
                    contentDialog.Content = "Transaction added fail";
                }
                contentDialog.CloseButtonText = "Close";
                await contentDialog.ShowAsync();
            }
            
        }
        private void Reset(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }
        private void ClearForm()
        {
            txtName.Text = "";
            txtDetail.Text = "";
            txtDescription.Text = "";
            category.Tag = null;
            txtAmount.Text = "";
            Date.Date = null;
            msgName.Text = "";
            msgDetail.Text = "";
            msgDescription.Text = "";
            msgCreatedAt.Text = "";
            msgCategory.Text = "";
            msgAmount.Text = "";
        }
        
        private bool ValidateForm()
        {
            validateCheck = 0;
            if (Validator.IsEmpty(txtName.Text))
            {
                msgName.Text = "Name is required";
                validateCheck += 1;
            }
            else
            {
                msgName.Text = "";
            }
            if (Validator.IsEmpty(txtDescription.Text))
            {
                msgDescription.Text = "Description is required";
                validateCheck += 1;
            }
            else
            {
                msgDescription.Text = "";
            }
            if (Validator.IsEmpty(txtDetail.Text))
            {
                msgDetail.Text = "Detail is required";
                validateCheck += 1;
            }
            else
            {
                msgDetail.Text = "";
            }
            if (Validator.IsEmpty(txtAmount.Text))
            {
                msgAmount.Text = "Amount is required";
                validateCheck += 1;
            }
            else
            {
                msgAmount.Text = "";
            }
            if (Validator.IsEmpty(Date.Date.ToString()))
            {
                msgCreatedAt.Text = "Created date is required";
                validateCheck += 1;
            }
            else
            {
                msgCreatedAt.Text = "";
            }
            if (Validator.IsEmpty(category.Tag.ToString()))
            {
                msgCategory.Text = "Category is required";
                validateCheck += 1;
            }
            else
            {
                msgCategory.Text = "";
            }
            return validateCheck > 0;
        }
        private void listViewCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category categorySelected = listViewCategory.SelectedItem as Category;
            category.Content = categorySelected.Name;
            category.Tag = categorySelected.Id;
            category.Flyout.Hide();
        }

    }
}
