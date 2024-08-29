using Fleck;
using System.Threading;
using System.Threading.Tasks;

namespace GoXLR.Server.Handlers.Commands
{
    public class CommandHandler
    {
        private readonly IWebSocketConnection _socket;

        public CommandHandler(IWebSocketConnection socket)
        {
            _socket = socket;
        }

        public async Task Send(CommandBase command, CancellationToken cancelationToken)
        {
            //Some commands needs multiple payloads:
            foreach (var json in command.Json)
            {
                await Send(json, cancelationToken);
            }
        }

        private async Task Send(string message, CancellationToken cancelationToken)
        {
            if (!_socket.IsAvailable || cancelationToken.IsCancellationRequested)
                return;
            
            await _socket?.Send(message);
        }
    }
}
