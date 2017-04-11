using IHK.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IHK.MultiUserBlock
{
    public class MultiUserBlockMiddlewareDebug
    {
        private readonly RequestDelegate _next;

        public MultiUserBlockMiddlewareDebug(RequestDelegate next)
        {
            _next = next;
            //_mubtHandler = multiUserBlockHandlerBase;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest || !context.User.IsInRole(UserRoleType.Admin.ToString()))
            {
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();

            Debug.WriteLine("Debug connected");

            await Receive(socket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await _receiveAsync(socket, result, buffer);
                    return;
                }

                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                              statusDescription: "Closed by the MultiUserBlockWebSocketManager",
                              cancellationToken: CancellationToken.None);

                    Debug.WriteLine("Debug disconnected");
                    return;
                }

            });

            //TODO - investigate the Kestrel exception thrown when this is the last middleware
            //await _next.Invoke(context);
        }

        private async Task _receiveAsync(WebSocket socket,WebSocketReceiveResult result, byte[] buffer)
        {
            Debug.WriteLine($"Debug said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");

            var message = JsonConvert.SerializeObject(MultiUserBlockManager._blocks);


            if (socket.State != WebSocketState.Open)
                return;

            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                                  offset: 0,
                                                                  count: message.Length),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None);

            return;
        }

        private async Task Receive(WebSocket sc, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while (sc.State == WebSocketState.Open)
            {
                var result = await sc.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                       cancellationToken: CancellationToken.None);

                handleMessage( result, buffer);
            }
        }
    }

}
