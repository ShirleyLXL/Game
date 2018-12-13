using Common;
using UnityEngine;

namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        private void OnRecvFriendsList(IChannel channel, Message message)
        {
            SFriendsList msg = message as SFriendsList;
            string friends_string = msg.friends_names;
            string[] friends_names = friends_string.Trim().Split(',');
            foreach (string names in friends_names) {
                if (names.Trim() == "") {
                    continue;
                }
                Debug.Log(names);
            }
            
            //TODO.... 可以调用UI里的函数创建一个个好友的item吗？...不会


        }
    }
}
