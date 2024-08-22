using _Models;
using Microsoft.AspNetCore.SignalR;

namespace Core.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user , string message)
        {
            await Clients.All.SendAsync("RecieveMessage", user, message);
        }


        //public async Task JoinChatRoom(string user, string message)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, con.ChatRoomId);
        //    await Clients.Group(con.ChatRoomId).SendAsync("ReceiveMessage", "Admin", $"{con.UserName} Has joined {con.ChatRoomId} !");
        //}
    }
}
