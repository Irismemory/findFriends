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

using findFriends.Model;
using findFriends.MyResources;


namespace findFriends.Bus
{
    public class HelpEventBus : INotifyPropertyChanged
    {
        private ToHelpEventData toHelpEventData;
        public HelpEventBus(string constructor)
        {
            toHelpEventData = new ToHelpEventData(constructor);
        }

        private ObservableCollection<HelpEventData> _allHelpEvent;
        public ObservableCollection<HelpEventData> AllHelpEvent
        {
            get {return _allHelpEvent;}
            set
            {
                _allHelpEvent = value;
                NotifyPropertyChanged("AllHelpEvent");
            }
        }

        public void SaveChangesToDB()
        {
            toHelpEventData.SubmitChanges();
        }

        public void SelectDB(int id)
        {
            var target = from HelpEventData inf in toHelpEventData.Items
                         where inf.ID == id
                         select inf;
            AllHelpEvent = new ObservableCollection<HelpEventData>(target);
        }


        public void SelectDB()
        {
            var target = from HelpEventData inf in toHelpEventData.Items
                         select inf;
            AllHelpEvent = new ObservableCollection<HelpEventData>(target);
        }

        public void insertDB(HelpEventData inf)
        {
            toHelpEventData.Items.InsertOnSubmit(inf);
            toHelpEventData.SubmitChanges();
            AllHelpEvent.Add(inf);
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
