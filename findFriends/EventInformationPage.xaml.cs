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
namespace findFriends
{
    public partial class EventInformationPage : PhoneApplicationPage
    {

        private ApplicationBarIconButton acceptButton;

        #region 构建方法
        public EventInformationPage()
        {
            InitializeComponent();

            if (Global.currentlyViewingEvent == null)
            {
                NavigationService.GoBack();
            }
            else
            {
                title.Text = Global.currentlyViewingEvent.Title;
                user.Text = Global.currentlyViewingEvent.User;
                description.Text = Global.currentlyViewingEvent.LongDescription;

                if (!Global.currentlyViewingEvent.IsSolved)
                {
                    setupApplicationBar();
                }
            }


        }
        #endregion

        void setupApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.IsVisible = true;
            ApplicationBar.Opacity = 0.5;
            ApplicationBar.IsMenuEnabled = true;
            ApplicationBar.BackgroundColor = System.Windows.Media.Colors.Black;

            acceptButton = new ApplicationBarIconButton(new Uri("Assets/AppBar/Send-AppBarIcon.png", UriKind.Relative));
            acceptButton.Text = "新建";
            acceptButton.Click += acceptButton_Click;
            ApplicationBar.Buttons.Add(acceptButton);
        }


        protected void acceptButton_Click(object sender, EventArgs e)
        {
            accept();
        }


        protected void accept()
        {
            //接受事件
        }
    }
}