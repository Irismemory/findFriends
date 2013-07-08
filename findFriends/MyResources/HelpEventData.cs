using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Net;

using System.Device.Location;

namespace findFriends.MyResources
{
    [Table]
    public class HelpEventData : INotifyPropertyChanged, INotifyPropertyChanging
    {

        private int id;
        [Column(IsPrimaryKey=true, IsDbGenerated=true, CanBeNull=false, AutoSync = AutoSync.OnInsert)]
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                if (id != value)
                {
                    NotifyPropertyChanging("ID");
                    id = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }

        private String title;
        [Column(CanBeNull=false)]
        public String Title 
        {
            get
            {
                return title;
            }
            set
            {
                if (title != value)
                {
                    NotifyPropertyChanging("ID");
                    title = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }

        private String shortDescription;
        public String ShortDescription
        {
            get
            {
                return shortDescription;
            }
        }

        private String longDescription;
        [Column(CanBeNull = false)]
        public String LongDescription
        {
            get
            {
                return longDescription;
            }
            set
            {
                if (longDescription != value)
                {
                    NotifyPropertyChanging("LongDescription");
                    longDescription = value;
                    shortDescription = value;
                    if (shortDescription.Length > maxShort)
                    {
                        shortDescription = shortDescription.Substring(0, maxShort) + "...";
                    }
                    NotifyPropertyChanged("LongDescription");
                }
            }
        }


        private DateTime time;
        [Column(CanBeNull = false)]
        public DateTime Time
        {
            set
            {
                if (time != value)
                {
                    NotifyPropertyChanging("Time");
                    time = value;
                    timeText = time.ToString("MM月dd日");
                    NotifyPropertyChanged("Time");
                }
            }
            get
            {
                return time;
            }
        }

        private String user;
        [Column(CanBeNull = false)]
        public String User
        {
            get
            {
                return user;
            }
            set
            {
                if (user != value)
                {
                    NotifyPropertyChanging("User");
                    user = value;
                    NotifyPropertyChanged("User");
                }
            }
        }

        private String timeText;
        public String TimeText
        {
            get
            {
                return timeText;
            }
        }


        private GeoCoordinate coordinate;
        public GeoCoordinate Coordinate
        {
            get
            {
                return new GeoCoordinate(latitude, longitude) ;
            }
        }

        private double latitude;
        [Column(CanBeNull = false)]
        public double Latitude
        {
            get
            {
                return latitude;
            }
            set
            {
                latitude = value;
                coordinate = new GeoCoordinate(latitude, longitude);
            }
        }

        private double longitude;
        [Column(CanBeNull = false)]
        public double Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                if (longitude != value)
                {
                    NotifyPropertyChanging("Longitude");
                    longitude = value;
                    NotifyPropertyChanging("Longitude");
                }
            }
        }

        private bool isSolved;
        [Column(CanBeNull=false)]
        public Boolean IsSolved
        {
            get 
            {
                return isSolved;
            }
            set
            {
                if (isSolved != value)
                {
                    NotifyPropertyChanging("IsSolved");
                    isSolved = value;
                    if (isSolved)
                    {
                        isSolvedText = "";
                    }
                    else
                    {
                        isSolvedText = "未解决";
                    }
                    NotifyPropertyChanged("IsSolved");
                }
            }
        }

        private String isSolvedText;
        public String IsSolvedText
        {
            get
            {
                if (isSolved) return "已解决";
                else return "";
            }
        }

        
        public const int maxShort = 30;
        /*
        public HelpEventData(int pId, String pTitle, String pLongDescription, String pUser, DateTime pTime, double pLatitude, double pLongitude, Boolean pIsSolved)
        {
            this.id = pId;
            this.Title = pTitle;
            this.LongDescription = pLongDescription;
            this.User = pUser;
            this.Time = pTime;
            this.longitude = pLongitude;
            this.latitude = pLatitude;
            coordinate = new GeoCoordinate(latitude, longitude);
            isSolved = pIsSolved;
                if (isSolved)
                {
                    isSolvedText = "";
                }
                else
                {
                    isSolvedText = "未解决";
                }
        }*/

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }
}
