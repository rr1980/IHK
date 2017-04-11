//using Microsoft.AspNetCore.Http;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.WebSockets;
//using System.Security.Claims;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace IHK.MultiUserBlock
//{
//    public abstract class MultiUserBlockHandlerBase
//    {
//        protected MultiUserBlockWebSocketManager MultiUserBlockWebSocketManager { get; set; }

//        public MultiUserBlockHandlerBase(MultiUserBlockWebSocketManager multiUserBlockWebSocketManager)
//        {
//            MultiUserBlockWebSocketManager = multiUserBlockWebSocketManager;
//        }

//        public virtual async Task OnConnected(WebSocket socket, HttpContext context)
//        {
//            var userId = Convert.ToInt32(context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
//            MultiUserBlockWebSocketManager.AddSocket(socket, userId);
//        }

//        public virtual async Task OnDisconnected(WebSocket socket, HttpContext context)
//        {
//            var userId = Convert.ToInt32(context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);
//            await MultiUserBlockWebSocketManager.RemoveSocket(MultiUserBlockWebSocketManager.GetSocketId(socket),userId);
//        }

//        public async Task SendMessageAsync(WebSocket socket, string message)
//        {
//            if (socket.State != WebSocketState.Open)
//                return;

//            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
//                                                                  offset: 0,
//                                                                  count: message.Length),
//                                   messageType: WebSocketMessageType.Text,
//                                   endOfMessage: true,
//                                   cancellationToken: CancellationToken.None);
//        }

//        public async Task SendMessageAsync(string socketId, string message)
//        {
//            await SendMessageAsync(MultiUserBlockWebSocketManager.GetSocketById(socketId), message);
//        }

//        public async Task SendMessageToAllAsync(string message)
//        {
//            var all = MultiUserBlockWebSocketManager.GetAll().Select(s=>s.ws);

//            foreach (var so in all)
//            {
//                if (so.State == WebSocketState.Open)
//                    await SendMessageAsync(so, message);
//            }
//        }

//        public abstract Task ReceiveAsync(WebSocket socket, HttpContext context, WebSocketReceiveResult result, byte[] buffer);
//    }
//}