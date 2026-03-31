using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Enters Extended Passive Mode. The server opens a port for a data connection.
/// Input: None.
/// Output: 229 (Entering Extended Passive Mode (|||<port>|)).
/// </summary>
public class EpsvCommand : IFtpCommand
{
    private readonly ILogger<EpsvCommand> _logger;

    public EpsvCommand(ILogger<EpsvCommand> logger)
    {
        _logger = logger;
    }

    public string Command => "EPSV";

    public async Task ExecuteAsync(FtpCommandContext context, string arg)
    {
        var listener = context.GetPassiveListener();
        var ep = (IPEndPoint)listener.LocalEndpoint;

        _logger.LogInformation("Extended Passive mode enabled on port {Port}", ep.Port);
        
        // Standard EPSV response format: (|||<port>|)
        await context.Writer.WriteLineAsync(
            $"229 Entering Extended Passive Mode (|||{ep.Port}|)");
    }
}
