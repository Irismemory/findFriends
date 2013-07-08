using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Net;

using findFriends.MyResources;

namespace findFriends.Model
{
    public class ToFriendData : DataContext
    {
        public ToFriendData(string constuctor)
            : base(constuctor)
        {
 
        }

        public Table<FriendData> Items;
    }
}
