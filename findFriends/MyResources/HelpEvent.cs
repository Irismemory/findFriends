using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace findFriends.MyResources
{
    public class HelpEvent
    {
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public String Title { get; set; }
        public String ShortDescription
        {
            get
            {
                return shortDescription;
            }
        }
        public String LongDescription
        {
            get
            {
                return longDescription;
            }
            set
            {
                longDescription = value;
                shortDescription = value;
                if (shortDescription.Length > maxShort)
                {
                    shortDescription = shortDescription.Substring(0, maxShort) + "...";
                }
            }
        }
        public DateTime Time
        {
            set
            {
                time = value;
                timeText = time.ToString("MM月dd日");
            }
            get
            {
                return time;
            }
        }
        public String User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
            }
        }
        public String TimeText
        {
            get
            {
                return timeText;
            }
        }
        public GeoCoordinate Coordinate
        {
            get
            {
                return coordinate;
            }
        }
        public double Latitude
        {
            get
            {
                return Latitude;
            }
            set
            {
                latitude = value;
                coordinate = new GeoCoordinate(latitude, longitude);
            }
        }

        public double Longitude
        {
            get
            {
                return longitude;
            }
            set
            {
                Longitude = value;
            }
        }

        public Boolean IsSolved
        {
            get 
            {
                return IsSolved;
            }
            set
            {
                isSolved = value;
                if (isSolved)
                {
                    isSolvedText = "";
                }
                else
                {
                    isSolvedText = "未解决";
                }
            }
        }

        public String IsSolvedText
        {
            get
            {
                return isSolvedText;
            }
        }

        private int id;
        private DateTime time;
        private String timeText;
        private String user;
        private String longDescription;
        private String shortDescription;
        private GeoCoordinate coordinate;
        private double latitude;
        private double longitude;
        private bool isSolved;
        private String isSolvedText;


        public const int maxShort = 30;

        public HelpEvent(int pId, String pTitle, String pLongDescription, String pUser, DateTime pTime, double pLatitude, double pLongitude, Boolean pIsSolved)
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
            
                
            
        }
    }
}
