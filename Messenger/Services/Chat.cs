using System.Net.WebSockets;

namespace Messenger.Services
{
    public class Chat
    {
        private static List<WebSocket> Client = new List<WebSocket>();

        internal static async Task Echo(HttpContext context, WebSocket webSocket, string name)
        {
            Client.Add(webSocket);
            var buffer = new byte[1024 * 4];
            await Console.Out.WriteLineAsync(name + " connected to the chat!");
            WebSocketReceiveResult wsresult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),
            CancellationToken.None);
            while (!wsresult.CloseStatus.HasValue)
            {
                await Console.Out.WriteLineAsync(name + " send message!");

                for(int i = 0; Client.Count > i; i++) 
                {
                    await Client[i].SendAsync(new ArraySegment<byte>(buffer, 0, wsresult.Count), wsresult.MessageType,
                wsresult.EndOfMessage, CancellationToken.None);
                }
                
                wsresult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            Client.Remove(webSocket);
            await webSocket.CloseAsync(wsresult.CloseStatus.Value, wsresult.CloseStatusDescription,
            CancellationToken.None);
            await Console.Out.WriteLineAsync(name + " disconnected from the chat!");
        }
    }
}
