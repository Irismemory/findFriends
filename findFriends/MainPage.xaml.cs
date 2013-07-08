using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using findFriends.Resources;

using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;

using System.Device.Location;
using System.Windows.Threading;


using System.IO.IsolatedStorage;

using findFriends.MyResources;
using findFriends.UserControls;


namespace findFriends
{
    public partial class MainPage : PhoneApplicationPage
    {
        /// <summary>
        /// 资源对象
        /// </summary>
        private MyResouces myResources = MyResouces.Instance;
        /// <summary>
        /// 地图监听对象
        /// </summary>
        GeoCoordinateWatcher watcher = null;

        /// <summary>
        /// 当前坐标的MapLayer
        /// </summary>
        private MapLayer localCoordinateMapLayer = null;


        //坐标点存储
        private IsolatedStorageSettings setting = IsolatedStorageSettings.ApplicationSettings;

        /// <summary>
        /// ApplicationBar新建按钮
        /// </summary>
        private ApplicationBarIconButton newEventButton = null;

        /// <summary>
        /// ApplicationBar设置按钮
        /// </summary>
        private ApplicationBarIconButton publishButton = null;

        /// <summary>
        /// ApplicationBar发布按钮
        /// </summary>
        private ApplicationBarIconButton localCoordinateButton = null;

        /// <summary>
        /// ApplicationBar定位按钮
        /// </summary>
        private ApplicationBarIconButton refreshButton = null;

        /// <summary>
        /// ApplicationBar注销帐号菜单项
        /// </summary>
        private ApplicationBarMenuItem CancelAccount = null;

        /// <summary>
        /// 用于解析坐标的位置信息
        /// </summary>
        private ReverseGeocodeQuery coordinateReverseGeocodeQuery = null;
        
        /// <summary>
        /// 地图坐标的MapLayer
        /// </summary>
        private MapLayer coordinateMapLayer = null;

        /// <summary>
        /// 路线信息
        /// </summary>
        private MapRoute mapRoute = null;

        /// <summary>
        /// 路线规划
        /// </summary>
        private RouteQuery routeQuery = null;


        #region 构造函数
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            // 用于本地化 ApplicationBar 的示例代码
            //BuildLocalizedApplicationBar();

            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            watcher.MovementThreshold = 5; // 20 meters
            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(OnStatusChanged);
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(OnPositionChanged);
            watcher.Start();
            BuildApplicationBar();
            getSettings();

         //   VisualStateGroup.CurrentStateChanged += VisualStateGroup_CurrentStateChanged;
        }
        #endregion

        #region  -初始化事件-

        //这是进行地图初步设置的地点
        //从缓存中读取上次地图定位的坐标，直接进入上次的定位效果
        //如果是第一次定位，那么默认进入中国北京地区

        private void getSettings()
        {
            if (setting.Contains("longtitude"))
            {
                double longtitude = (double)setting["longtitude"];
                double latitude = (double)setting["latitude"];
                MyMap.ZoomLevel = 18;
                myResources.LocalGeoCoordinate =
                   new GeoCoordinate(latitude, longtitude);
                MyMap.Center = myResources.LocalGeoCoordinate;
                DrawMapLayer(myResources.LocalGeoCoordinate);
                MyMap.SetView(myResources.LocalGeoCoordinate, MyMap.ZoomLevel, myResources.AnimationKind);
            }
            else
            {
                MyMap.ZoomLevel = 14;
                myResources.LocalGeoCoordinate =
                  new GeoCoordinate(39.926588, 116.411133);
                MyMap.Center = myResources.LocalGeoCoordinate;
                DrawMapLayer(myResources.LocalGeoCoordinate);
                MyMap.SetView(myResources.LocalGeoCoordinate, MyMap.ZoomLevel, myResources.AnimationKind);
                
            }
        }

        #endregion 
        
        #region -位置改变检测-

        void OnPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {

            double accuracy = e.Position.Location.HorizontalAccuracy;

            if (accuracy < e.Position.Location.VerticalAccuracy)
            {
                accuracy = e.Position.Location.VerticalAccuracy;
            }
            Debug.WriteLine("locationa ccuracy :" + accuracy);
            myResources.LocalGeoCoordinate= new GeoCoordinate(e.Position.Location.Latitude+0.001426 , e.Position.Location.Longitude + 0.006201);
            DrawMapLayer(myResources.LocalGeoCoordinate);
            MyMap.SetView(myResources.LocalGeoCoordinate, MyMap.ZoomLevel, myResources.AnimationKind);
        }

        void OnStatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            Debug.WriteLine("OnStatusChanged: " + e.ToString());   
        }

        #endregion

        #region 在地图上画出当前坐标的MapLayer
      
        void DrawMapLayer(GeoCoordinate coordinate)
        {
            if (coordinate == null) throw new ArgumentNullException("Local coordinate cannot be set null.");
            if (localCoordinateMapLayer != null)
            {
                if (MyMap.Layers.Contains(localCoordinateMapLayer))
                {
                    MyMap.Layers.Remove(localCoordinateMapLayer);
                }
                localCoordinateMapLayer = null;
            }
            localCoordinateMapLayer = new MapLayer();

            // 创建显示当前坐标的标签
            LocalCoordinateLabel label = new LocalCoordinateLabel();
            label.Tag = new GeoCoordinate(coordinate.Latitude, coordinate.Longitude);
            label.Tap += CoordinateLabel_Click;

            // 创建MapOverlay并添加标签
            MapOverlay overlay = new MapOverlay();
            overlay.Content = label;
            overlay.GeoCoordinate = new GeoCoordinate(coordinate.Latitude, coordinate.Longitude);
            overlay.PositionOrigin = new Point(0.5, 0.5);
            localCoordinateMapLayer.Add(overlay);
            MyMap.Layers.Add(localCoordinateMapLayer);
        }
        private void CoordinateLabel_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
           GeoCoordinate coordinate = (sender as LocalCoordinateLabel).Tag as GeoCoordinate;
        }

        #endregion

        #region - ApplicationBar构建 -

        /// <summary>
        /// 创建ApplicationBar
        /// </summary>
        private void BuildApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBar.Mode = ApplicationBarMode.Default;
            ApplicationBar.IsVisible = true;
            ApplicationBar.Opacity = 0.7;
            ApplicationBar.IsMenuEnabled = true;
            ApplicationBar.BackgroundColor = System.Windows.Media.Colors.Black;

            // 创建ApplicationBar的Buttons


            newEventButton = 
                new ApplicationBarIconButton(new Uri("Assets/AppBar/New-AppBarIcon.png", UriKind.Relative));
            newEventButton.Text = "新建";
            newEventButton.Click += newEventButton_Click;
            ApplicationBar.Buttons.Add(newEventButton);

            publishButton =
                new ApplicationBarIconButton(new Uri("/Assets/AppBar/Send-AppBarIcon.png", UriKind.Relative));
            publishButton.Text = "发布";
            publishButton.Click += PublishButton_Click;
            ApplicationBar.Buttons.Add(publishButton);

            localCoordinateButton =
                new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.locate.me.png", UriKind.Relative));
            localCoordinateButton.Text = "定位";
            localCoordinateButton.Click += LocalCoordinateButton_Click;
            ApplicationBar.Buttons.Add(localCoordinateButton);

            refreshButton =
                new ApplicationBarIconButton(new Uri("/Assets/AppBar/Refresh-AppBarIcon.png", UriKind.Relative));
            refreshButton.Text = "刷新";
            refreshButton.Click += RefreshButton_Click;
            ApplicationBar.Buttons.Add(refreshButton);

            CancelAccount =
                new ApplicationBarMenuItem("切换登陆账号");
            CancelAccount.Click += CancelAccount_Click;
            ApplicationBar.MenuItems.Add(CancelAccount);
        }

        private void newEventButton_Click(object sender, System.EventArgs e)
        {
            Global.switchPage(this, "/NewEventPage.xaml");
        }

        private void PublishButton_Click(object sender, System.EventArgs e)
        {
                
        }

        private void LocalCoordinateButton_Click(object sender, EventArgs e)
        {
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {

        }

        private void CancelAccount_Click(object sender, EventArgs e)
        {

        }


        #endregion

        #region -Override 事件-

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

         //   GoToState(MapShowState);
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

        }
        #endregion

        #region 多个位置坐标的加载以及显示
        /// <summary>
        /// 在地图上画出坐标的MapLayer
        /// </summary>
        /// <param name="location"></param>
        private void DrawCoordinateMapLayer(IList<MapLocation> locations)
        {
            if (locations == null) throw new ArgumentNullException("Locations cannot be set null.");

            ClearCoordinateMapLayer();

            coordinateMapLayer = new MapLayer();

            foreach (MapLocation location in locations)
            {
                // 创建显示当前坐标的标签
              //  CoordinateLabel label = new CoordinateLabel();
           //     label.Location = location;
            //    label.OnRouteTap += label_OnRouteTap;

                // 创建MapOverlay并添加标签
                MapOverlay overlay = new MapOverlay();
           //     overlay.Content = label;
                overlay.GeoCoordinate = new GeoCoordinate(location.GeoCoordinate.Latitude, location.GeoCoordinate.Longitude);
                overlay.PositionOrigin = new Point(0.5, 1.0);
                coordinateMapLayer.Add(overlay);
            }

            MyMap.Layers.Add(coordinateMapLayer);
        }
        private void label_OnRouteTap(object sender, EventArgs e)
        {
            if (myResources.LocalGeoCoordinate == null)
            {
               // MessageBox.Show(AppResources.NoLocalCoordinateInfoText, AppResources.ApplicationTitle, MessageBoxButton.OK);
            }
            else
            {

                if (mapRoute != null)
                {
                    MyMap.RemoveRoute(mapRoute);
                    mapRoute = null;
                }
                myResources.RouteLegs.Clear();

             //   GeoCoordinate targetCoordinate = (sender as MapLabel).Location.GeoCoordinate;
             //   myResources.RouteTargetMapLocation = (sender as MapLabel).Location;

                List<GeoCoordinate> coordinates = new List<GeoCoordinate>();
                coordinates.Add(myResources.LocalGeoCoordinate);
            //    coordinates.Add(targetCoordinate);
                CalculateRoute(coordinates,TravelMode.Walking);
            }
        }
        /// <summary> 
        /// 清除在地图上画的MapLayer
        /// </summary>
        private void ClearCoordinateMapLayer()
        {
            if (coordinateMapLayer != null)
            {
                if (MyMap.Layers.Contains(coordinateMapLayer))
                {
                    MyMap.Layers.Remove(coordinateMapLayer);
                }
                coordinateMapLayer = null;
            }
        }

        /// <summary>
        /// 路线规划
        /// </summary>
        /// <param name="route"></param>
        private void CalculateRoute(List<GeoCoordinate> route, TravelMode travelMode)
        {

            if (routeQuery == null || !routeQuery.IsBusy)
            {
                routeQuery = new RouteQuery();
                routeQuery.TravelMode = travelMode;
                routeQuery.Waypoints = route;
                routeQuery.QueryCompleted += RouteQuery_QueryCompleted;
                routeQuery.QueryAsync();
            }
        }
        private void RouteQuery_QueryCompleted(object sender, QueryCompletedEventArgs<Route> e)
        {
            if (e.Error == null && e.Result != null)
            {
                Route route = e.Result;
                mapRoute = new MapRoute(route);
                MyMap.AddRoute(mapRoute);

                foreach (RouteLeg leg in route.Legs)
                {
                    myResources.RouteLegs.Add(leg);
                }

                MyMap.SetView(route.Legs[0].BoundingBox, myResources.AnimationKind);
                this.Focus();
            }       
        }
        #endregion

    }

    #region 朋友和事件的测试数据
    public class HelpEventListBoxCollection : ObservableCollection<HelpEventData>
    {
        public HelpEventListBoxCollection()
        {
        }

    }

    public class StaticHelpEventListBoxCollection : HelpEventListBoxCollection
    {
        public StaticHelpEventListBoxCollection()
        {
            Add(new HelpEventData(0, "没带纸", "", "康妍", DateTime.Now, 60, 60, false));
            Add(new HelpEventData(1, "需要搭便车", "上一个事件没有描述。", "范家琦", DateTime.Now, 60, 60, true));
            Add(new HelpEventData(3, "猫咪在树上", "上一个事件的描述比较短。这是一个比较长的描述。啦啦啦啦", "杨含泊", DateTime.Now, 60, 60, true));
            Add(new HelpEventData(2, "猫咪在树上", "这是一个更加长的描述啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦啦", "杨含泊", DateTime.Now, 60, 60, false));
            
        }
    }

    public class FriendListBoxCollection : ObservableCollection<FriendData>
    {
        public FriendListBoxCollection()
        {
        }
    }

    public class StaticFriendListBoxCollection : FriendListBoxCollection
    {
        public StaticFriendListBoxCollection()
        {
            Add(new FriendData("康妍", "kangyan@email.com"));
            Add(new FriendData("范家琦", "fanjiaqi@email.com"));
            Add(new FriendData("杨含泊", "yanghanbo@email.com"));
            Add(new FriendData("杨含泊", "yanghanbo@email.com"));

            Add(new FriendData("杨含泊", "yanghanbo@email.com"));
            Add(new FriendData("杨含泊", "yanghanbo@email.com"));
        }
    }
#endregion 

}

