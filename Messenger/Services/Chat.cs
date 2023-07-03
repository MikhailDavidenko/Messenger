using System.Net.WebSockets;
using System.Text;

namespace Messenger.Services
{
    public class Chat
    {
        private static List<WebSocket> Users = new List<WebSocket>();

        internal static async Task Echo(HttpContext context, WebSocket webSocket, string name)
        {
            Users.Add(webSocket);

            var buffer = new byte[1024 * 4];
            await Console.Out.WriteLineAsync(name + " connected to the chat!");
            //Wait a message
            WebSocketReceiveResult wsresult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer),
            CancellationToken.None);

            while (!wsresult.CloseStatus.HasValue)
            {
                string message = name+": "+ Encoding.UTF8.GetString(buffer, 0, wsresult.Count);
                await Console.Out.WriteLineAsync(name + " send message!");

                //send message to all connected users
                for (int i = 0; Users.Count > i; i++)
                {
                    if (Users[i] != webSocket)
                    {
                        await Users[i].SendAsync(new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                                  offset: 0,
                                                                  count: message.Length), 
                    wsresult.MessageType,
                    wsresult.EndOfMessage, CancellationToken.None);
                    }
                }
                
                wsresult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            Users.Remove(webSocket);
            await webSocket.CloseAsync(wsresult.CloseStatus.Value, wsresult.CloseStatusDescription,
            CancellationToken.None);
            await Console.Out.WriteLineAsync(name + " disconnected from the chat!");
        }

    }
}
