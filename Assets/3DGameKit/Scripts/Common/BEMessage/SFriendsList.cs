using System;

namespace Common
{
    [Serializable]
    public class SFriendsList : Message
    {
        public SFriendsList() : base(Command.S_FRIENDS_LIST) { }
        public string EntityID;   //暂时没用到
        public string user;   //暂时没用到
        public string friends_names;
    }
}
