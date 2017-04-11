using IHK.Common;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IHK.MultiUserBlock
{

    public class MultiUserBlockMiddleware
    {
        private readonly RequestDelegate _next;

        public MultiUserBlockMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest || !context.User.IsInRole(UserRoleType.Default.ToString()))
            {
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var userId = Convert.ToInt32(context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            //var sc = MultiUserBlockManager.OnConnected(socket, userId);

            await Receive(socket, async (webSocket, result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var tmp_msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var rmsg = JsonConvert.DeserializeObject<ReceiveMsg>(tmp_msg);
                    await MultiUserBlockManager.ReceiveAsync(webSocket, rmsg);
                    return;
                }

                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await MultiUserBlockManager.OnDisconnected(webSocket, userId);
                    return;
                }

            });

            //TODO - investigate the Kestrel exception thrown when this is the last middleware
            //await _next.Invoke(context);
        }
        
        private async Task Receive(WebSocket socket,Action<WebSocket,WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                       cancellationToken: CancellationToken.None);

                handleMessage(socket, result, buffer);
            }
        }
    }

}
