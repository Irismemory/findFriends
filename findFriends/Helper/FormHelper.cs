using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using System.Windows.Input;

namespace findFriends.Helper
{
    public class FormHelper
    {
        

        public static void combineBoxWithWatermark(TextBox textbox, TextBlock textblock)
        {

            textbox.TextChanged += delegate(object sender, TextChangedEventArgs e)
            {
                if (textbox.Text != "")
                {
                    textblock.Opacity = 0.0;
                }
                else textblock.Opacity = 1.0;
            };
            return;

        }
        public static void combineBoxWithWatermark(PasswordBox passwordbox, TextBlock textblock)
        {

            passwordbox.PasswordChanged += delegate(object sender, RoutedEventArgs e)
            {
                if (passwordbox.Password != "")
                {
                    textblock.Opacity = 0;
                }
                else
                {
                    textblock.Opacity = 1;
                }
            };
            return;

        }

        public static void chainBoxesByEnterKey(TextBox a, TextBox b)
        {

            a.KeyDown += delegate(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    b.Focus();
                }
            };
        }
        public static void chainBoxesByEnterKey(TextBox a, PasswordBox b)
        {

            a.KeyDown += delegate(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    b.Focus();
                }
            };
        }
        public static void chainBoxesByEnterKey(PasswordBox a, PasswordBox b)
        {

            a.KeyDown += delegate(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    b.Focus();
                }
            };
        }

        public static void bindBoxWithFunctionViaEnterKey(TextBox box, Action myFunc)
        {
            box.KeyDown += delegate(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    myFunc();
                }
            };
        }
    }
}
