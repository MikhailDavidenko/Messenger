using Messenger;
using Messenger.Services;
using System.Net.WebSockets;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.UseWebSockets();
        app.UseMiddleware<WSocketMiddleware>();

        app.MapGet("/", () => "Hello World!");

       

        app.Run();
    }
}