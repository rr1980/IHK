using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IHK.MultiUserBlock
{
    public class MultiUserBlockMiddleware
    {
        private readonly RequestDelegate _next;
        private MultiUserBlockHandlerBase _mubtHandler { get; set; }

        public MultiUserBlockMiddleware(RequestDelegate next,
                                          MultiUserBlockHandlerBase multiUserBlockHandlerBase)
        {
            _next = next;
            _mubtHandler = multiUserBlockHandlerBase;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            await _mubtHandler.OnConnected(socket, context);

            await Receive(socket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await _mubtHandler.ReceiveAsync(socket, context, result, buffer);
                    return;
                }

                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _mubtHandler.OnDisconnected(socket, context);
                    return;
                }

            });

            //TODO - investigate the Kestrel exception thrown when this is the last middleware
            //await _next.Invoke(context);
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                       cancellationToken: CancellationToken.None);

                handleMessage(result, buffer);
            }
        }
    }

}
