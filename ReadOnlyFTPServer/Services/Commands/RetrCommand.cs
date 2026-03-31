using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Retrieves (downloads) a file from the server over a data connection.
/// Input: The name of the file to retrieve.
/// Output: 150 (Starting transfer), then data, then 226 (Success).
/// </summary>
public class RetrCommand : IFtpCommand
{
    private readonly ILogger<RetrCommand> _logger;

    public RetrCommand(ILogger<RetrCommand> logger)
    {
        _logger = logger;
    }

    public string Command => "RETR";

    public async Task ExecuteAsync(FtpCommandContext context, string arg)
    {
        if (!context.LoggedIn)
        {
            await context.Writer.WriteLineAsync("530 Not logged in");
            return;
        }

        if (context.PassiveListener == null)
        {
            await context.Writer.WriteLineAsync("425 Use PASV first");
            return;
        }

        var filePath = context.ResolvePath(arg);
        if (!File.Exists(filePath))
        {
            _logger.LogWarning("Client requested non-existent file: {Path}", filePath);
            await context.Writer.WriteLineAsync("550 File not found");
            return;
        }

        await context.Writer.WriteLineAsync("150 Opening data connection");

        try
        {
            using (var dataClient = await context.PassiveListener.AcceptTcpClientAsync())
            using (var dataStream = dataClient.GetStream())
            using (var fs = File.OpenRead(filePath))
            {
                if (context.RestartOffset > 0 && context.RestartOffset < fs.Length)
                {
                    fs.Seek(context.RestartOffset, SeekOrigin.Begin);
                    _logger.LogInformation("Resuming RETR at offset {Offset}", context.RestartOffset);
                }

                _logger.LogInformation("Streaming file: {Path} from offset {Offset}", filePath, fs.Position);
                await fs.CopyToAsync(dataStream);
                context.RestartOffset = 0; // Consumed
            }

            await context.Writer.WriteLineAsync("226 Transfer complete");
        }
        finally
        {
            context.PassiveListener.Stop();
            context.PassiveListener = null;
        }
    }
}
