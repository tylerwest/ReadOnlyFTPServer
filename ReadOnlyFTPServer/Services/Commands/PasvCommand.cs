using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Enters passive mode. The server opens a port for a data connection.
/// Input: None.
/// Output: 227 (Entering Passive Mode with IP and Port).
/// </summary>
public class PasvCommand : IFtpCommand
{
    private readonly ILogger<PasvCommand> _logger;

    public PasvCommand(ILogger<PasvCommand> logger)
    {
        _logger = logger;
    }

    public string Command => "PASV";

    public async Task ExecuteAsync(FtpCommandContext context, string arg)
    {
        var listener = context.GetPassiveListener();
        var ep = (IPEndPoint)listener.LocalEndpoint;
        var addr = ((IPEndPoint)context.Client.Client.LocalEndPoint!).Address.GetAddressBytes();
        var p1 = ep.Port / 256;
        var p2 = ep.Port % 256;

        _logger.LogInformation("Passive mode enabled on port {Port}", ep.Port);
        await context.Writer.WriteLineAsync(
            $"227 Entering Passive Mode ({addr[0]},{addr[1]},{addr[2]},{addr[3]},{p1},{p2})");
    }
}
