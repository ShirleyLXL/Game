using System;

namespace Common
{
    [Serializable]
    public class CFriendsList : Message
    {
        public CFriendsList() : base(Command.C_FRIENDS_LIST) { }
        public int userID;      //只发送userID
    }
}
