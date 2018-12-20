using Common;
using Backend.Game;
using System;

namespace Backend.Network
{
    public partial class Incoming
    {
        private void OnRecvChatMessage(IChannel channel, Message message)
        {
            CChatMessage request = message as CChatMessage;
            SChatMessage response = new SChatMessage();

            //int sender_entityId = request.sender_entityId;
            Console.WriteLine("I receive the message");
            string receiver_userName = request.receiver_userName;

            //查看对方是否在线，不在线则需要存到数据库（暂时没有实现）
            if (Global.user_entityIds == null || !Global.user_entityIds.ContainsKey(receiver_userName)) {
                Console.WriteLine("对方没有上线");
                return;
            }
            int receiver_entityId = Global.user_entityIds[receiver_userName];

            Console.WriteLine(request.sender_userName + " prepare to send " + request.receiver_userName + " entityId: " + receiver_entityId);
            
            //利用receiver_entityId来建立channel
            Player receiver = World.Instance.GetEntity(receiver_entityId) as Player;
            IChannel channel2 = receiver.connection;

            response.content = request.content;
            response.name = request.sender_userName;

            channel2.Send(response);

        }
    }
}
