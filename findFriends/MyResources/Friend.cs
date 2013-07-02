using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace findFriends.MyResources
{
    public class Friend
    {
        public String Nickname
        {
            get
            {
                return nickname;
            }
        }
        public String Email
        {
            get
            {
                return email;
            }
        }
        private String nickname;
        private String email;

        public Friend(String pNickname, String pEmail)
        {
            nickname = pNickname;
            email = pEmail;
        }
    }
}
