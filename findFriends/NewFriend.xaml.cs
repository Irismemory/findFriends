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
    public partial class NewFriend : PhoneApplicationPage
    {
        public NewFriend()
        {
            InitializeComponent();

            FormHelper.combineBoxWithWatermark(nameTB, nameWM);
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            submit();
        }

        private void submit()
        {
        }
    }
}