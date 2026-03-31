using System.Text;
using Microsoft.Extensions.Logging;

namespace ReadOnlyFTPServer.Services.Commands;

/// <summary>
/// Purpose: Lists files and directories in the current working directory over a data connection.
/// Input: None (or optional path).
/// Output: 150 (Connection start), then data, then 226 (Closing connection).
/// </summary>
public class ListCommand : IFtpCommand
{
    private readonly ILogger<ListCommand> _logger;

    public ListCommand(ILogger<ListCommand> logger)
    {
        _logger = logger;
    }

    public string Command => "LIST";

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

        await context.Writer.WriteLineAsync("150 Opening data connection");

        try
        {
            using (var dataClient = await context.PassiveListener.AcceptTcpClientAsync())
            using (var dataStream = dataClient.GetStream())
            using (var dw = new StreamWriter(dataStream, Encoding.ASCII) { NewLine = "\r\n", AutoFlush = true })
            {
                var di = new DirectoryInfo(context.CurrentDir);
                var items = di.GetFileSystemInfos();
                
                _logger.LogDebug("Listing directory: {Dir} ({Count} items)", context.CurrentDir, items.Length);

                foreach (var item in items)
                {
                    await dw.WriteLineAsync(FormatLine(item));
                }
            }

            await context.Writer.WriteLineAsync("226 Transfer complete");
        }
        finally
        {
            context.PassiveListener.Stop();
            context.PassiveListener = null;
        }
    }

    private static string FormatLine(FileSystemInfo info)
    {
        var isDir = info is DirectoryInfo;
        var type = isDir ? "d" : "-";
        var size = (info as FileInfo)?.Length ?? 0;
        
        var date = info.LastWriteTime;
        const string dateFormat = "MMM dd HH:mm";

        return $"{type}rwxr-xr-x 1 owner group {size,10} {date.ToString(dateFormat)} {info.Name}";
    }
}
