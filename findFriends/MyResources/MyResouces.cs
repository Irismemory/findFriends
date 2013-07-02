using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findFriends.MyResources
{
    public class MyResouces : BindingableDate
    {
        /// <summary>
        ///  唯一实例
        /// </summary>
        public static MyResouces Instance { get { return App.Current.Resources["MyResouces"] as MyResouces; } }

        private MapAnimationKind _animationKind = MapAnimationKind.None;
        /// <summary>
        /// 地图调整的动画效果
        /// </summary>
        public MapAnimationKind AnimationKind
        {
            get { return _animationKind; }
            set
            {
                if (_animationKind != value)
                {
                    _animationKind = value;
                    NotifyPropertyChanged("AnimationKind");
                }
            }
        }

        private GeoCoordinate _localGeoCoordinate = null;
        /// <summary>
        /// 当前的坐标
        /// </summary>
        public GeoCoordinate LocalGeoCoordinate
        {
            get { return _localGeoCoordinate; }
            set
            {
                if (_localGeoCoordinate != value)
                {
                    _localGeoCoordinate = value;
                    NotifyPropertyChanged("LocalGeoCoordinate");
                }
            }
        }

        private ObservableCollection<MapLocation> _coordinatePostions = null;
        /// <summary>
        /// 坐标可能所在的位置信息
        /// </summary>
        public ObservableCollection<MapLocation> CoordinatePostions
        {
            get
            {
                if (_coordinatePostions == null)
                {
                    _coordinatePostions = new ObservableCollection<MapLocation>();
                }
                return _coordinatePostions;
            }
        }

        private ObservableCollection<MapLocation> _searchResultLocations = null;
        /// <summary>
        /// 位置搜索结果信息
        /// </summary>
        public ObservableCollection<MapLocation> SearchResultLocations
        {
            get
            {
                if (_searchResultLocations == null)
                {
                    _searchResultLocations = new ObservableCollection<MapLocation>();
                }
                return _searchResultLocations;
            }
        }

        private MapLocation _routeTargetMapLocation = null;
        /// <summary>
        /// 路线规划目标位置
        /// </summary>
        public MapLocation RouteTargetMapLocation
        {
            get { return _routeTargetMapLocation; }
            set
            {
                if (_routeTargetMapLocation != value)
                {
                    _routeTargetMapLocation = value;
                    NotifyPropertyChanged("RouteTargetMapLocation");
                }
            }
        }

        private ObservableCollection<RouteLeg> _routeLegs = null;
        /// <summary>
        /// 路线规划详细信息
        /// </summary>
        public ObservableCollection<RouteLeg> RouteLegs
        {
            get
            {
                if (_routeLegs == null)
                {
                    _routeLegs = new ObservableCollection<RouteLeg>();
                }
                return _routeLegs;
            }
        }
    }
}
