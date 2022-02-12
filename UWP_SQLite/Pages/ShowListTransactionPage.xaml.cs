using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class ShowListTransactionPage : Page
    {
        public ShowListTransactionPage()
        {
            this.Loaded += ShowListTransactionPage_Loaded;
            this.InitializeComponent();
        }

        private void ShowListTransactionPage_Loaded(object sender, RoutedEventArgs e)
        {
            var list = Data.DatabaseInitialize.ShowList();
            ListView listView = new ListView();
            listView.Items.Add("Name - Description - Detail - Amount - CreatedAt - Category");
            foreach (var transaction in list)
            {
                listView.Items.Add(transaction.ToString());
            }
            List.Children.Add(listView);
        }
    }
}
