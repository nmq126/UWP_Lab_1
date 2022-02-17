﻿
using SQLitePCL;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP_SQLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static Frame frame;
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += Navigation_Loaded;
            frame = contentFrame;
        }

        public void Navigation_Loaded(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(Pages.CreateTransactionPage));
        }

        private void nvSample_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            NavigationViewItemBase item = args.InvokedItemContainer;
            switch (item.Tag)
            {
                case "CreateTransaction":
                    _ = contentFrame.Navigate(typeof(Pages.CreateTransactionPage));
                    break;
                case "ShowTransaction":
                    _ = contentFrame.Navigate(typeof(Pages.ShowListTransactionPage));
                    break;
            }
        }
    }
}

