using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using Newtonsoft.Json.Linq;
using System.IO;
using System.IO.IsolatedStorage;

using System.Windows.Input;
using System.Text;
using findFriends.Helper;
using findFriends.MyResources;

namespace findFriends
{
    public partial class LoginPage : PhoneApplicationPage
    {
        private IsolatedStorageSettings _appSetting;

        private const String _userNameKey = "username";
        private const String _passwordKey = "password";

        public LoginPage()
        {
            InitializeComponent();

            FormHelper.combineBoxWithWatermark(userNameTextBox, userNameWatermark);
            FormHelper.combineBoxWithWatermark(passwordBox, passwordWatermark);
            FormHelper.chainBoxesByEnterKey(userNameTextBox, passwordBox);

            //registerButton.Click += registerButton_Click;
            //loginButton.Click += loginButton_Click;

            userNameErrorMessage.Text = "";
            passwordErrorMessage.Text = "";

            userNameTextBox.LostFocus += delegate(object sender, RoutedEventArgs e)
            {
                if (userNameTextBox.Text != "" && userNameErrorMessage.Text != "")
                {
                    userNameErrorMessage.Text = "";
                }
            };

            passwordBox.LostFocus += delegate(object sender, RoutedEventArgs e)
            {
                if (passwordBox.Password != "" && passwordErrorMessage.Text != "")
                {
                    passwordErrorMessage.Text = "";
                }
            };

            passwordBox.KeyDown += delegate(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    submit();
                }
            };

            _appSetting = IsolatedStorageSettings.ApplicationSettings;

            if (_appSetting.Contains(_userNameKey))
            {
                userNameTextBox.Text = (String)_appSetting[_userNameKey];
                userNameWatermark.Opacity = 0;

            }
            if (_appSetting.Contains(_passwordKey))
            {
                passwordBox.Password = (String)_appSetting[_passwordKey];
                passwordWatermark.Opacity = 0;
            }


        }



        private void registerButton_Click(object sender, EventArgs e)
        {
            Global.switchPage(this, "/RegisterPage.xaml");
        }


        private void submit()
        {


            String UserName = userNameTextBox.Text;
            String Password = passwordBox.Password;
            Boolean RememberPassword = (Boolean)rememberPasswordCheckbox.IsChecked;

            if (UserName == "")
            {
                userNameTextBox.Focus();
                userNameErrorMessage.Text = "用户名不得为空";
                return;
            }
            userNameErrorMessage.Text = "";
            if (Password == "")
            {
                passwordBox.Focus();
                passwordErrorMessage.Text = "密码不得为空";
                return;
            }
            passwordErrorMessage.Text = "";

            Boolean loginSuccessful = false;



            byte[] encodedPassword = System.Text.Encoding.UTF8.GetBytes(UserName + ":" + Password);
            String encodedAuto = System.Convert.ToBase64String(encodedPassword);
            //判断
            Uri uri = new Uri("http://172.16.94.177:8001/test");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.Headers["Authorization"] = "Basic " + encodedAuto;
            request.BeginGetRequestStream(new AsyncCallback(a =>
            {
                var httprequest = a.AsyncState as HttpWebRequest;
                var stream = httprequest.EndGetRequestStream(a);


                var streamWriter = new StreamWriter(stream);
                streamWriter.Write("UserName=" + UserName);
                streamWriter.Write("Password=" + Password);
                streamWriter.Close();

                httprequest.BeginGetResponse(new AsyncCallback(b =>
                {
                    try
                    {
                        var httpWebRequest3 = b.AsyncState as HttpWebRequest;

                        WebResponse webResponse = httpWebRequest3.EndGetResponse(b);
                        var s = webResponse.ContentType;

                        var stream3 = webResponse.GetResponseStream();

                        var streamReader = new StreamReader(stream3, Encoding.UTF8);
                        var text = streamReader.ReadToEnd();

                        Dispatcher.BeginInvoke(() => MessageBox.Show(text));

                        loginSuccessful = true;


                    }
                    catch (Exception ex)
                    {
                        var c = ex.Message;
                        Dispatcher.BeginInvoke(() => MessageBox.Show(c));

                    }
                }), httprequest);
            }
            ), request);


            if (!loginSuccessful) return;

            if (_appSetting.Contains(_userNameKey))
            {
                _appSetting[_userNameKey] = UserName;
            }
            else
            {
                _appSetting.Add(_userNameKey, UserName);
            }

            if (RememberPassword)
            {
                if (_appSetting.Contains(_passwordKey))
                {
                    _appSetting[_passwordKey] = Password;
                }
                else
                {
                    _appSetting.Add(_passwordKey, Password);
                }
            }
            else
            {
                if (_appSetting.Contains(_passwordKey))
                {
                    _appSetting.Remove(_passwordKey);
                }
            }

            Global.switchPage(this, "/MainPage.xaml");
        }


        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            submit();
        }

        private void ApplicationBarIconButton_Click_2(object sender, EventArgs e)
        {
            Global.switchPage(this, "/RegisterPage.xaml");
        }


    }


}