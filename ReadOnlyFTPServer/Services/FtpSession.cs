using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Logging;
using ReadOnlyFTPServer.Models;

namespace ReadOnlyFTPServer.Services;

public class FtpSession
{
    private readonly Dictionary<string, IFtpCommand> _commandMap;
    private readonly TcpClient _client;
    private readonly FtpServerOptions _options;
    private readonly UserStore _userStore;
    private readonly ILogger<FtpSession> _logger;

    public FtpSession(TcpClient client,
        FtpServerOptions options,
        UserStore userStore,
        ILogger<FtpSession> logger,
        IEnumerable<IFtpCommand> commands)
    {
        _client = client;
        _options = options;
        _userStore = userStore;
        _logger = logger;
        _commandMap = commands.ToDictionary(c => c.Command, c => c, StringComparer.OrdinalIgnoreCase);
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        using var stream = _client.GetStream();
        using var reader = new StreamReader(stream, Encoding.ASCII, false, 1024, true);
        using var writer = new StreamWriter(stream, Encoding.ASCII, 1024, true);
        writer.NewLine = "\r\n";
        writer.AutoFlush = true;

        var context = new FtpCommandContext
        {
            Writer = writer,
            Options = _options,
            UserStore = _userStore,
            Client = _client,
            CurrentDir = Path.GetFullPath(_options.RootPath)
        };

        try
        {
            await writer.WriteLineAsync("220 Read-only FTP Server Ready");

            while (!cancellationToken.IsCancellationRequested)
            {
                string? line;
                try 
                { 
                    line = await reader.ReadLineAsync(cancellationToken); 
                }
                catch (OperationCanceledException) { break; }
                catch { break; }

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
                var cmdText = parts[0].ToUpperInvariant();
                var arg = parts.Length > 1 ? parts[1] : "";

                _logger.LogInformation("Command: {Command} {Argument}", cmdText, cmdText == "PASS" ? "********" : arg);

                if (_commandMap.TryGetValue(cmdText, out var command))
                {
                    await command.ExecuteAsync(context, arg);
                    if (cmdText == "QUIT") break;
                }
                else
                {
                    await writer.WriteLineAsync("502 Command not implemented");
                }
            }
        }
        finally
        {
            context.PassiveListener?.Stop();
            _logger.LogInformation("Session closed");
        }
    }
}
