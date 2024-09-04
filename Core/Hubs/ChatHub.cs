using _DataAccess.Repository.IRepository;
using _Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using NuGet.Protocol.Plugins;
namespace Core.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IUserService userService;
        private readonly IUnitOfWork unitOfWork;

        public ChatHub(IUserService userService , IUnitOfWork unitOfWork )
        {
            this.userService = userService;
            this.unitOfWork = unitOfWork;
        }

        //Invoked Automatically whenever a Client connect to Chat hub
        public async override Task<Task> OnConnectedAsync()
        {
            string youId = userService.GetCurrentUserId();
            string youName = userService.GetCurrentUserName();
            await Clients.Others.SendAsync("Announce", youName, " Is Online");
            UserHandler.ConnectedIds.Add(youId);

            // get id of the user that you want to message
            var path = Context.GetHttpContext().Request.Path.ToString();
            string recieverId = path.Substring(path.LastIndexOf('/') + 1);

            //check if there's any unread messages from db
            List<UnreadMessages> unreadmessages = unitOfWork.UnreadMessagesRepo.GetAll(x => x.FromId == recieverId && x.ToId == youId).ToList();
            
            // send those messages and delete them from db
            foreach (var message in unreadmessages)
            {
                await Clients.Caller.SendAsync("RecieveMessage" , message.Message , "reciever");
                unitOfWork.UnreadMessagesRepo.Remove(message);
            }
            unitOfWork.Save();
            return base.OnConnectedAsync();
        }

        //Invoked Automatically whenever a Client disconnect from Chat hub
        public async override Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            string youId = userService.GetCurrentUserId();
            string youName = userService.GetCurrentUserName();
            UserHandler.ConnectedIds.Remove(youId);
            await Clients.Others.SendAsync("Announce", youName, " Is Offline");
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMsg(string sender, string reciever, string message)
        {
            string senderName = userService.GetCurrentUserName();
            if (!UserHandler.ConnectedIds.Contains(reciever))
            {
                UnreadMessages messagetodb = new UnreadMessages
                {
                    FromId = userService.GetCurrentUserId(),
                    ToId = reciever,
                    Message = message,
                    SendDate = DateTime.Now,
                };
                unitOfWork.UnreadMessagesRepo.Add(messagetodb);
                unitOfWork.Save();
            }
            else
            {
                await Clients.User(reciever).SendAsync("RecieveMessage", message ,"reciever");
            }
            await Clients.Caller.SendAsync("RecieveMessage", message , "sender");
        }
    
    }

    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }
}


//public async Task JoinChatRoom(string user, string message)
//{
//    await Groups.AddToGroupAsync(Context.ConnectionId, con.ChatRoomId);
//    await Clients.Group(con.ChatRoomId).SendAsync("ReceiveMessage", "Admin", $"{con.UserName} Has joined {con.ChatRoomId} !");
//}
//public async Task SendMessage(string user , string message)
//{
//    await Clients.All.SendAsync("RecieveMessage", user, message);
//    await Clients.Caller.SendAsync(message);
//    await Clients.Others.SendAsync(message);
//    await Clients.Client("Id").SendAsync(message);
//    await Clients.AllExcept("ID").SendAsync(message);
//    await Clients.User("username or ID").SendAsync(message);
//}