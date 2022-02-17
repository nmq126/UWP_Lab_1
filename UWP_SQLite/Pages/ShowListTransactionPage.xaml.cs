using System;
using System.Collections.Generic;
using System.Diagnostics;
using UWP_SQLite.Entity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP_SQLite.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShowListTransactionPage : Page
    {
        private List<PersonalTransaction> transactions = new List<PersonalTransaction>();
        public List<PersonalTransaction> listTransaction;
        public static PersonalTransaction personal;
        private string checkedStartDate;
        private string checkedEndDate;
        public ShowListTransactionPage()
        {
            this.InitializeComponent();
            this.Loaded += ShowListTransactionPage_Loaded;
        }

        private void ShowListTransactionPage_Loaded(object sender, RoutedEventArgs e)
        {           
            listCategoryData.ItemsSource = Data.DatabaseInitialize.ShowListCategory();
            transactions = Data.DatabaseInitialize.ShowList();
            DataGridView.ItemsSource = transactions;
            //dataTransaction.ItemsSource = transactions;
            //btnTotalMoney.Text = Server.DataInitialization.totalMoney.ToString();
            double total = 0;
            foreach (PersonalTransaction t in transactions)
            {
                total += t.Amount;
            }
            TotalAmount.Text = " " + total.ToString();

        }
        private void listViewCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category categorySelection = listCategoryData.SelectedItem as Category;
            searchCategory.Content = categorySelection.Name;
            searchCategory.Tag = categorySelection.Id;
            searchCategory.Flyout.Hide();
        }
        private async void Show(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            foreach (PersonalTransaction t in transactions)
            {
                if (t.Id == (int)button.Tag)
                {
                    Debug.WriteLine("ok");
                }
            }
        }
        //private readonly List<PersonalTransaction> transactions = new List<PersonalTransaction>();
        //public ShowListTransactionPage()
        //{
        //    this.InitializeComponent();
        //    transactions = Data.DatabaseInitialize.ShowList();
        //    DataGridView.ItemsSource = transactions;
        //    double total = 0;
        //    foreach (PersonalTransaction t in transactions)
        //    {
        //        total += t.Amount;
        //    }
        //    TotalExpenditure.Text = total.ToString("c");
        //}

        //private void Search(object sender, RoutedEventArgs e)
        //{
        //    int count = 0;
        //    if (StartDate.Date == null)
        //    {
        //        StartDateError.Text = "Please Enter start date!";
        //        count += 1;
        //    }
        //    if (EndDate.Date == null)
        //    {
        //        EndDateError.Text = "Please Enter end date!";
        //        count += 1;
        //    }
        //    if (StartDate.Date != null && EndDate.Date != null && StartDate.Date > EndDate.Date)
        //    {
        //        StartDateError.Text = "";
        //        EndDateError.Text = "Please Enter end date > start date!";
        //        count += 1;
        //    }
        //    if (count == 0)
        //    {
        //        StartDateError.Text = "";
        //        EndDateError.Text = "";
        //        DateTimeOffset? sDate = StartDate.Date;
        //        DateTime sTime = sDate.Value.DateTime;
        //        string startDate = sTime.ToString("yyyy-MM-dd");
        //        DateTimeOffset? eDate = EndDate.Date;
        //        DateTime eTime = eDate.Value.DateTime;
        //        string endDate = eTime.ToString("yyyy-MM-dd");
        //        List<PersonalTransaction> transaction1 = Data.DatabaseInitialize.SearchByDay(startDate, endDate);
        //        double total = 0;
        //        foreach (PersonalTransaction t in transaction1)
        //        {
        //            total += t.Amount;
        //        }
        //        TotalExpenditure.Text = total.ToString("c");
        //        DataGridView.ItemsSource = transaction1;
        //    }
        //}

        //private async void Show(object sender, RoutedEventArgs e)
        //{
        //    Button button = sender as Button;
        //    foreach (PersonalTransaction t in transactions)
        //    {
        //        if (t.Id == (int)button.Tag)
        //        {
        //            Debug.WriteLine("ok");
        //        }
        //    }
        //}
    }
}
