using System.Threading.Tasks;
using Fleck;
using Loupedeck.GoXLR.Plugin.GoXLR.Commands;

namespace Loupedeck.GoXLR.Plugin.GoXLR
{
    public static class CommandHandler
    {
        /// <summary>
        /// Send Commands to the GoXLR Software
        /// </summary>
        /// <param name="commands">Commands</param>
        /// <param name="wsConnection">WebSocket Connection</param>
        public static async Task Send(CommandBase commands, IWebSocketConnection wsConnection)
        {
            foreach (var command in commands.Json)
            {
                await Send(command, wsConnection);
            }
        }

        /// <summary>
        /// Send Command to the GoXLR Software
        /// </summary>
        /// <param name="command">Serialized Json String</param>
        /// <param name="wsConnection">WebSocket Connection</param>
        private static async Task Send(string command, IWebSocketConnection wsConnection)
        {
            await wsConnection.Send(command);
        }
    }
}