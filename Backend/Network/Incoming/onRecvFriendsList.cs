using Common;
using System;
using Backend.Game;
using Npgsql;

namespace Backend.Network
{
    public partial class Incoming
    {
        private void OnRecvFriendsList(IChannel channel, Message message)
        {
            //前端发送一个带有userID的消息，后端从数据库将好友信息取出并作为消息发给前端
            CFriendsList request = message as CFriendsList;
            SFriendsList response = new SFriendsList();
            int userID = request.userID;     //为了方便debug，接受到的userID都是1
            String friendslist = "";
            

            //连接数据库
            String connString = "Host=localhost;Port=5432;Username=postgres;Password=mmmmmm;Database=game";
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            System.Console.WriteLine("success to connect to database");

            var cmd = new NpgsqlCommand(string.Format("SELECT friendName FROM FriendsList WHERE userID = {0}", userID), conn);
            var reader = cmd.ExecuteReader();
            
            //读出查询结果的每一行
            if (!reader.HasRows) {
                System.Console.WriteLine("result is none");
                return;
            }
            while (reader.Read()) {
                System.Console.WriteLine(reader.GetString(0));
                friendslist = friendslist + reader.GetString(0) + ",";
            }
            conn.Close();

            //消息发送给前端
            System.Console.WriteLine("friendalist: " + friendslist);
            response.friends_names = friendslist;
            channel.Send(response);

        }
    }
}
