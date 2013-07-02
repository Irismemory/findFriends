using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using findFriends.MyResources;
using findFriends.Helper;

namespace findFriends
{
    public partial class theFirstPageofApp : PhoneApplicationPage
    {
        public theFirstPageofApp()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Global.switchPage(this, "/LoginPage.xaml");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Global.switchPage(this, "/MainPage.xaml");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }


    }
}