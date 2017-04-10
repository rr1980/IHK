using IHK.Common;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace IHK.MultiUserBlock
{
    public class MultiUserBlockHandler : MultiUserBlockHandlerBase
    {
        public MultiUserBlockHandler(MultiUserBlockWebSocketManager multiUserBlockWebSocketManager) : base(multiUserBlockWebSocketManager)
        {
        }

        public override async Task OnConnected(WebSocket socket, HttpContext context)
        {
            await base.OnConnected(socket, context);

            var socketId = MultiUserBlockWebSocketManager.GetSocketId(socket);
            await SendMessageToAllAsync($"{socketId} is now connected");
        }

        public override async Task ReceiveAsync(WebSocket socket, HttpContext context, WebSocketReceiveResult result, byte[] buffer)
        {
            if (!context.User.IsInRole(UserRoleType.Default.ToString()))
            {
                return;
            }

            var socketId = MultiUserBlockWebSocketManager.GetSocketId(socket);
            var message = $"{socketId} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";

            Debug.WriteLine(message);

            await SendMessageToAllAsync(message);
        }
    }
}