using Common;
using UnityEngine;

namespace Gamekit3D.Network
{
    public partial class Incoming
    {
        public GameObject messageContent;
        ChatUI chat_handler;
        private void OnRecvChatMessage(IChannel channel, Message message)
        {
            SChatMessage msg = message as SChatMessage;
            messageContent = GameObject.Find("MessageContent");
            chat_handler = messageContent.GetComponent<ChatUI>();
            chat_handler.ReceiveFriendMessage(msg.name, msg.content);
            
        }
    }
}
