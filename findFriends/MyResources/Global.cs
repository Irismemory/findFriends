﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;


namespace findFriends.MyResources
{
    class Global
    {
        public static HelpEvent currentlyViewingEvent = null;
        public static void switchPage(PhoneApplicationPage page, string pageName)
        {
            page.NavigationService.Navigate(new Uri(pageName, UriKind.Relative));
        }
    }
}
