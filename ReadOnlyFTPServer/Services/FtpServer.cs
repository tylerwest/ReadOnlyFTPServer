namespace ReadOnlyFTPServer.Services;

using System.Net;
using System.Net.Sockets;
using Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class FtpServer : BackgroundService
{
    private readonly FtpServerOptions _options;
    private readonly ILogger<FtpServer> _logger;
    private readonly ILogger<FtpSession> _sessionLogger;
    private readonly UserStore _userStore;
    private readonly IEnumerable<IFtpCommand> _commands;

    public FtpServer(ILogger<FtpServer> logger,
        ILogger<FtpSession> sessionLogger,
        IOptions<FtpServerOptions> options,
        UserStore userStore,
        IEnumerable<IFtpCommand> commands)
    {
        _logger = logger;
        _sessionLogger = sessionLogger;
        _userStore = userStore;
        _commands = commands;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Directory.CreateDirectory(_options.RootPath);

        var listener = new TcpListener(IPAddress.Any, _options.Port);
        listener.Start();

        _logger.LogInformation("FTP server listening on port {Port}", _options.Port);

        while (!stoppingToken.IsCancellationRequested)
        {
            var client = await listener.AcceptTcpClientAsync(stoppingToken);
            var remoteEndPoint = client.Client.RemoteEndPoint?.ToString() ?? "unknown";
            
            _ = Task.Run(async () =>
            {
                using var scope = _logger.BeginScope(new Dictionary<string, object> { ["RemoteAddress"] = remoteEndPoint });
                _logger.LogInformation("New client connected from {RemoteAddress}", remoteEndPoint);
                
                var session = new FtpSession(client, _options, _userStore, _sessionLogger, _commands);
                await session.RunAsync(stoppingToken);
            }, stoppingToken);
        }
    }
}
