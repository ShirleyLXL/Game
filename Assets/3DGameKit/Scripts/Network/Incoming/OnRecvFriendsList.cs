using Common;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        public GameObject friendListContent;
        Text friendNameText;
        FriendUI handler;
        private void OnRecvFriendsList(IChannel channel, Message message)
        {
            friendListContent = GameObject.Find("FriendListContent");
            handler = friendListContent.GetComponent<FriendUI>();
            
            if (friendListContent == null || handler == null)
            {
                Debug.Log("content null");
                return;
            }
            
            //FGlobal存储前端的全局变量
            FGlobal.friendList = new ArrayList();
            SFriendsList msg = message as SFriendsList;
            string friends_string = msg.friends_names;
            string[] friends_names = friends_string.Trim().Split(',');
            foreach (string names in friends_names) {
                if (names.Trim() == "") {
                    continue;
                }
                FGlobal.friendList.Add(names);
                
                //Debug.Log(names);
            }
            //handler调用FriendUI中的方法
            handler.AddFriendList();
            foreach (string names in FGlobal.friendList) {
                Debug.Log(names);    
            }


        }
    }
}
