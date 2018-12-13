using System;

namespace Common
{
    [Serializable]
    public class CFriendsList : Message
    {
        public CFriendsList() : base(Command.C_FRIENDS_LIST) { }
        public int userID;    //这个消息可以用专门发送userID的消息来代替
    }
}
