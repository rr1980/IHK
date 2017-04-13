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
using Newtonsoft.Json.Serialization;
using IHK.MultiUserBlock.Interfaces;

namespace IHK.MultiUserBlock
{
    /// <summary>
    /// AspNetCore MiddleWare um paralelle mehrfach Zugriffe auf Entitys zu verhindern/verwalten
    /// </summary>
    public class MultiUserBlockMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMultiUserBlockManager _multiUserBlockManager;




        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="multiUserBlockManager"></param>
        public MultiUserBlockMiddleware(RequestDelegate next, IMultiUserBlockManager multiUserBlockManager)
        {
            _next = next;
            _multiUserBlockManager = multiUserBlockManager;
        }




        /// <summary>
        /// AspNetCore Zugriffspunkt
        /// </summary>
        /// <param name="context">HttpContext von Asp Net Core des aktuellen Request</param>
        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest || !context.User.IsInRole(UserRoleType.Default.ToString()))
            {
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var userId = Convert.ToInt32(context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            _multiUserBlockManager.OnConnected(socket, userId);

            await Receive(socket, async (webSocket, result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var tmp_msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var rmsg = JsonConvert.DeserializeObject<MultiUserBlockReceiveMessage>(tmp_msg);
                    await _multiUserBlockManager.ReceiveAsync(webSocket, rmsg);
                    return;
                }

                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _multiUserBlockManager.OnDisconnected(webSocket, userId);
                    return;
                }

            });

            //TODO - investigate the Kestrel exception thrown when this is the last middleware
            //await _next.Invoke(context);
        }




        /// <summary>
        /// AspNetCore Zugriffspunkt
        /// </summary>
        /// <param name="socket">Websocket des Clienten</param>
        /// <param name="handleMessage">Action mit der die Anfrage verarbeitet wird</param>
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
