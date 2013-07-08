using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.Text;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using findFriends.Helper;
using findFriends.MyResources;

namespace findFriends
{
    public partial class RegisterPage : PhoneApplicationPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            FormHelper.combineBoxWithWatermark(nickNameTB, nickNameWM);
            FormHelper.combineBoxWithWatermark(emailTB, emailWM);
            FormHelper.combineBoxWithWatermark(passwordTB, passwordWM);
            FormHelper.combineBoxWithWatermark(rePasswordTB, rePasswordWM);

            FormHelper.chainBoxesByEnterKey(nickNameTB, emailTB);
            FormHelper.chainBoxesByEnterKey(emailTB, passwordTB);
            FormHelper.chainBoxesByEnterKey(passwordTB, rePasswordTB);

            nickNameErrorMessage.Text = "";
            emailErrorMessage.Text = "";
            passwordErrorMessage.Text = "";
            rePasswordErrorMessage.Text = "";

            nickNameTB.LostFocus += delegate(object sender, RoutedEventArgs e)
            {
                if (nickNameTB.Text != "" && nickNameErrorMessage.Text != "")
                {
                    nickNameErrorMessage.Text = "";
                }
            };
            emailTB.LostFocus += delegate(object sender, RoutedEventArgs e)
            {
                if (emailTB.Text != "" && emailErrorMessage.Text != "")
                {
                    emailErrorMessage.Text = "";
                }
            };
            passwordTB.LostFocus += delegate(object sender, RoutedEventArgs e)
            {
                if (passwordTB.Password != "" && passwordErrorMessage.Text != "")
                {
                    passwordErrorMessage.Text = "";
                }
            };
            rePasswordTB.LostFocus += delegate(object sender, RoutedEventArgs e)
            {
                if (rePasswordTB.Password != "" && rePasswordErrorMessage.Text != "")
                {
                    if (rePasswordTB.Password == passwordTB.Password)
                    {
                        rePasswordErrorMessage.Text = "";
                    }
                    else
                    {
                        rePasswordErrorMessage.Text = "必须与密码相同";
                    }
                }
            };


            rePasswordTB.KeyDown += delegate(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Enter)
                {
                    submit();
                }
            };
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Global.switchPage(this, "/LoginPage.xaml");
        }

        private void submit()
        {
            String Nickname = nickNameTB.Text;
            String Password = passwordTB.Password;
            String Email = emailTB.Text;
            String rePassword = rePasswordTB.Password;

            if (Nickname == "")
            {
                nickNameTB.Focus();
                nickNameErrorMessage.Text = "昵称不得为空";
                return;
            }
            else if (Email == "")
            {
                emailTB.Focus();
                emailErrorMessage.Text = "邮箱不得为空";
                return;
            }
            else if (Password == "")
            {
                passwordTB.Focus();
                passwordErrorMessage.Text = "密码不得为空";
                return;
            }
            else if (rePassword == "")
            {
                rePasswordTB.Focus();
                rePasswordErrorMessage.Text = "必须重复输入密码";
                return;
            }
            else if (Password != rePassword)
            {
                rePasswordTB.Focus();
                rePasswordErrorMessage.Text = "必须与密码相同";
                return;
            }

            Boolean registerSuccessful = false;

            Uri uri = new Uri("http://172.16.94.177:8001/user/create/");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.BeginGetRequestStream(new AsyncCallback(a =>
            {
                var httprequest = a.AsyncState as HttpWebRequest;
                var stream = httprequest.EndGetRequestStream(a);


                var streamWriter = new StreamWriter(stream);
                JObject job = new JObject();
                job.Add("name", Nickname);
                job.Add("password", Password);
                streamWriter.WriteLine(job.ToString());
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

                        registerSuccessful = true;
                    }
                    catch (Exception ex)
                    {
                        var c = ex.Message;
                        Dispatcher.BeginInvoke(() => MessageBox.Show(c + "fsedfdsfsd"));

                    }
                }), httprequest);
            }
            ), request);


            if (!registerSuccessful) return;

            MessageBox.Show("注册成功!");

            this.NavigationService.GoBack();
        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            submit();
        }
    }
}