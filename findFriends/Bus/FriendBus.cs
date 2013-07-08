using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Net;

using System.Diagnostics;

using findFriends.Model;
using findFriends.MyResources;


namespace findFriends.Bus
{

    public class FriendBus : INotifyPropertyChanged
    {
        private ToFriendData toFriendData;
        public FriendBus(String constructor)
        {
            toFriendData = new ToFriendData(constructor);
        }

        private ObservableCollection<FriendData> _allFriends;
        public ObservableCollection<FriendData> AllFriends
        {
            get { return _allFriends; }
            set
            {
                _allFriends = value;
                NotifyPropertyChanged("AllFriends");
            }
        }

        public void SaveChangesToDB()
        {
            toFriendData.SubmitChanges();
        }

        public void SelectDB(String nickname)
        {
            var target = from FriendData inf in toFriendData.Items
                         where inf.Nickname == nickname
                         select inf;
            AllFriends = new ObservableCollection<FriendData>(target);
        }


        public void SelectDB()
        {
            var target = from FriendData inf in toFriendData.Items
                         select inf;
            AllFriends = new ObservableCollection<FriendData>(target);
        }

        public void insertDB(FriendData inf)
        {
            toFriendData.Items.InsertOnSubmit(inf);
            toFriendData.SubmitChanges();
            AllFriends.Add(inf);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }


}