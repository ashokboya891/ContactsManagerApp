using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
    using Microsoft.AspNetCore.SignalR;
    using System.Collections.Concurrent;
using Entities;
using ContactsManager.Core.Domain.Entities;

namespace ContactsManager.Infrastructure.Repositories
{

    public class ChatHub : Hub
    {

        private static readonly ConcurrentDictionary<string, string> Users = new ConcurrentDictionary<string, string>();

        private readonly ApplicationDbContext _dbContext;

        public ChatHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override Task OnConnectedAsync()
        {
            // Map the connection ID to the username
            var userName = Context.User.Identity.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                Users[Context.ConnectionId] = userName;
                UpdateUserList();
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            // Remove user on disconnection
            if (Users.TryRemove(Context.ConnectionId, out _))
            {
                UpdateUserList();
            }

            return base.OnDisconnectedAsync(exception);
        }


        public async Task SendMessageToUser(string receiverConnectionId, string senderName, string message)
        {
            // Resolve the receiver's name from the connection ID
            if (Users.TryGetValue(receiverConnectionId, out var receiverUser))
            {
                // Save message to the database
                var chatMessage = new ChatMessage
                {
                    Sender = senderName,
                    Receiver = receiverUser,
                    Message = message,
                    Timestamp = DateTime.UtcNow
                };

                _dbContext.ChatMessages.Add(chatMessage);
                await _dbContext.SaveChangesAsync();

                // Send the message to the receiver
                await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", senderName, message);

                // Send the message to the sender
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", senderName, message);
            }
            else
            {
                // Handle case where receiver is not found (e.g., user disconnected)
                await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", "System", $"User not found or offline.");
            }
        }



        private void UpdateUserList()
        {
            foreach (var user in Users)
            {
                Console.WriteLine($"User: {user.Value}, ConnectionId: {user.Key}");
            }

            var userList = Users.Select(u => new { connectionId = u.Key, name = u.Value }).ToList();
            Clients.All.SendAsync("UpdateUserList", userList);
        }

    }

}
