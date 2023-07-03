using Messenger.Services;
using System.Net.WebSockets;

namespace Messenger
{
    public class WSocketMiddleware
    {
        private readonly RequestDelegate _next;

        public WSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }

            if (context.Request.Path == "/ws")
            {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    string name = context.Request.Query["name"];
                    await Chat.Echo(context, webSocket, name);
                
            }
        }
    }

}
