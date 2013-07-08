using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Net;
using System;

namespace findFriends.MyResources
{
    [Table]
    public class FriendData : INotifyPropertyChanged, INotifyPropertyChanging
    {
        
        private String nickname;
        [Column(IsPrimaryKey = true, IsDbGenerated = false, CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public String Nickname
        {
            set
            {
                if (nickname != value)
                {
                    NotifyPropertyChanging("Nickname");
                    nickname = value;
                    NotifyPropertyChanged("Nickname");
                }
            }
            get
            {
                return nickname;
            }
        }

        private String email;
        [Column(CanBeNull = true)]
        public String Email
        {
            set
            {
                if (email != value)
                {
                    NotifyPropertyChanging("Email");
                    email = value;
                    NotifyPropertyChanged("Email");
                }
            }
            get
            {
                return email;
            }
        }


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
