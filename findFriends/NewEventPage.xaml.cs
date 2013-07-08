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

using Windows.Devices.Geolocation;
using System.Device.Location;

using System.Windows.Threading;
using System.Threading.Tasks;

using findFriends.Helper;
using findFriends.MyResources;



namespace findFriends
{
    public partial class NewEventPage : PhoneApplicationPage
    {

        Geolocator locator;
        public NewEventPage()
        {
            InitializeComponent();


            titleError.Text = "";
            descriptionError.Text = "";
            descriptionTB.LostFocus += delegate(object sender, RoutedEventArgs e)
            {
                if (descriptionTB.Text != "" && descriptionError.Text != "")
                {
                    descriptionError.Text = "";
                }
            };

            titleTB.LostFocus += delegate(object sender, RoutedEventArgs e)
            {
                if (titleTB.Text != "" && titleError.Text != "")
                {
                    titleError.Text = "";
                }
            };

            locator = new Geolocator();
            locator.DesiredAccuracyInMeters = 50;
            locator.MovementThreshold = 5;
            locator.ReportInterval = 500;

            #region Stylization
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
            #endregion

        }

        private void ApplicationBarIconButton_Click_1(object sender, EventArgs e)
        {
            submit();
        }

        private async void submit()
        {

            String Title = titleTB.Text;
            String Description = descriptionTB.Text;

            #region Checking TextBoxes

            if (Title == "")
            {
                titleError.Text = "事件标题不得为空";
                titleTB.Focus();
                return;
            }
            if (Description == "")
            {
                descriptionError.Text = "事件描述不得为空";
                descriptionTB.Focus();
                return;
            }
            #endregion

            Geoposition currentPosition = null;

            try
            {
                MessageBox.Show("Trying to get position.");
                currentPosition = await locator.GetGeopositionAsync();
            }
            catch (System.UnauthorizedAccessException)
            {
                MessageBox.Show("Acquiring position failed.");
                return;
            }
            catch (TaskCanceledException)
            {
                MessageBox.Show("Cancelled acquiring position.");
                return;
            }


            Double Longitude = currentPosition.Coordinate.Longitude;
            Double Latitude = currentPosition.Coordinate.Latitude;

            MessageBox.Show(Longitude + ", " + Latitude);
            int X = (int)Math.Floor(Longitude);
            int Y = (int)Math.Floor(Latitude);

            int X_tag = (int)Math.Floor((Longitude - X) * 1000000);
            int Y_tag = (int)Math.Floor((Latitude - Y) * 1000000);
            

            Uri uri = new Uri("http://objpy.sinaapp.com/createHelp/");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.BeginGetRequestStream(new AsyncCallback(a =>
            {
                var httprequest = a.AsyncState as HttpWebRequest;
                var stream = httprequest.EndGetRequestStream(a);


                var streamWriter = new StreamWriter(stream);
                JObject job = new JObject();
                job.Add("name", Global.currentUser);
                job.Add("x", X);
                job.Add("y", Y);
                job.Add("x_tag", X_tag);
                job.Add("y_tag", Y_tag);
                job.Add("help_title", Title);
                job.Add("help_content", Description);
                job.Add("kind", "");
                job.Add("is_soluted", 0);

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

                        Dispatcher.BeginInvoke(() => MessageBox.Show("创建成功"));
                    }
                    catch (Exception ex)
                    {
                        var c = ex.Message;
                        Dispatcher.BeginInvoke(() => MessageBox.Show(c + " (Failed)"));

                    }
                }), httprequest);
            }
            ), request);


        }



    }
}