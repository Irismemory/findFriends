using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using findFriends.Helper;

namespace findFriends
{
    public partial class NewEventPage : PhoneApplicationPage
    {
        public NewEventPage()
        {
            InitializeComponent();

            FormHelper.combineBoxWithWatermark(titleTB, titleWM);
            FormHelper.combineBoxWithWatermark(descriptionTB, descriptionWM);
            FormHelper.chainBoxesByEnterKey(titleTB, descriptionTB);
            FormHelper.bindBoxWithFunctionViaEnterKey(descriptionTB, submit);

            titleTB.GotFocus += delegate(object sender, RoutedEventArgs e)
            {
                ApplicationBar.Opacity = 1.0;
            };
            descriptionTB.GotFocus += delegate(object sender, RoutedEventArgs e)
            {
                ApplicationBar.Opacity = 1.0;
            };

            titleTB.LostFocus += delegate(object sender, RoutedEventArgs e)
            {
                ApplicationBar.Opacity = 0.5;
            };
            descriptionTB.LostFocus += delegate(object sender, RoutedEventArgs e)
            {
                ApplicationBar.Opacity = 0.5;
            };

        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            submit();
        }

        private void submit()
        {
            MessageBox.Show("Submitting");
        }



    }
}