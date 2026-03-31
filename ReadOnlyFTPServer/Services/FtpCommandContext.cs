using System.Net;
using System.Net.Sockets;
using ReadOnlyFTPServer.Models;

namespace ReadOnlyFTPServer.Services;

public class FtpCommandContext
{
    public required StreamWriter Writer { get; init; }
    public required FtpServerOptions Options { get; init; }
    public required UserStore UserStore { get; init; }
    public required TcpClient Client { get; init; }

    public string CurrentDir { get; set; } = "";
    public long RestartOffset { get; set; } = 0;
    public bool LoggedIn { get; set; }
    public string? PendingUser { get; set; }
    public TcpListener? PassiveListener { get; set; }

    public TcpListener GetPassiveListener()
    {
        PassiveListener?.Stop();

        for (int i = 0; i < 100; i++) // Try up to 100 times
        {
            var port = Random.Shared.Next(Options.PassivePortRangeLow, Options.PassivePortRangeHigh + 1);
            try
            {
                var listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                PassiveListener = listener;
                return listener;
            }
            catch
            {
                continue;
            }
        }
        throw new InvalidOperationException("No passive ports available in range.");
    }

    public string ResolvePath(string arg)
    {
        var path = arg.StartsWith('/')
            ? Path.Combine(Options.RootPath, arg.TrimStart('/'))
            : Path.Combine(CurrentDir, arg);

        path = Path.GetFullPath(path);
        return path.StartsWith(Options.RootPath, StringComparison.OrdinalIgnoreCase)
            ? path
            : Options.RootPath;
    }

    public string GetRelativePath(string path)
    {
        var rel = Path.GetRelativePath(Options.RootPath, path);
        return rel == "." ? "/" : "/" + rel.Replace("\\", "/");
    }
}
