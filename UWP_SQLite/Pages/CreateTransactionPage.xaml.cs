using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_SQLite.Entity;
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
        public CreateTransactionPage()
        {
            this.InitializeComponent();
        }

        private void Submit(object sender, RoutedEventArgs e)
        {

            PersonalTransaction transaction = new PersonalTransaction();
            transaction.Name = txtName.Text;
            transaction.Description = txtDescription.Text;
            transaction.Detail = txtDetail.Text;
            transaction.Amount = Double.Parse(txtAmount.Text);
            transaction.Category = int.Parse(txtCategory.Text);
            transaction.CreatedAt = date;
            Data.DatabaseInitialize.InsertData(transaction);
        }
        private void CalendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            CalendarDatePicker cdp = sender as CalendarDatePicker;
            date = cdp.Date.Value.DateTime;
        }
    }
}
