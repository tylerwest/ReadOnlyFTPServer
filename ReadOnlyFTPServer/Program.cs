using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ReadOnlyFTPServer.Models;
using ReadOnlyFTPServer.Services;
using ReadOnlyFTPServer.Services.Commands;

var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
});

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("server.json", optional: false, reloadOnChange: true)
    .AddJsonFile("users.json", optional: false, reloadOnChange: true);

builder.Services.Configure<FtpServerOptions>(builder.Configuration);
builder.Services.Configure<UserStoreOptions>(builder.Configuration);

builder.Services.AddSingleton<UserStore>();

// Auth commands
builder.Services.AddSingleton<IFtpCommand, UserCommand>();
builder.Services.AddSingleton<IFtpCommand, PassCommand>();

// Connection commands
builder.Services.AddSingleton<IFtpCommand, TypeCommand>();
builder.Services.AddSingleton<IFtpCommand, FeatCommand>();
builder.Services.AddSingleton<IFtpCommand, SystCommand>();
builder.Services.AddSingleton<IFtpCommand, PasvCommand>();
builder.Services.AddSingleton<IFtpCommand, EpsvCommand>();
builder.Services.AddSingleton<IFtpCommand, OptsCommand>();
builder.Services.AddSingleton<IFtpCommand, QuitCommand>();

// Stream commands
builder.Services.AddSingleton<IFtpCommand, RetrCommand>();
builder.Services.AddSingleton<IFtpCommand, RestCommand>();

// Read commands
builder.Services.AddSingleton<IFtpCommand, PwdCommand>();
builder.Services.AddSingleton<IFtpCommand, CwdCommand>();
builder.Services.AddSingleton<IFtpCommand, CdupCommand>();
builder.Services.AddSingleton<IFtpCommand, ListCommand>();
builder.Services.AddSingleton<IFtpCommand, NoopCommand>();
builder.Services.AddSingleton<IFtpCommand, SizeCommand>();
builder.Services.AddSingleton<IFtpCommand, MdtmCommand>();

// Write commands
builder.Services.AddSingleton<IFtpCommand, StorCommand>();
builder.Services.AddSingleton<IFtpCommand, AppeCommand>();
builder.Services.AddSingleton<IFtpCommand, DeleCommand>();
builder.Services.AddSingleton<IFtpCommand, MkdCommand>();
builder.Services.AddSingleton<IFtpCommand, RmdCommand>();
builder.Services.AddSingleton<IFtpCommand, RnfrCommand>();
builder.Services.AddSingleton<IFtpCommand, RntoCommand>();

builder.Services.AddHostedService<FtpServer>();

var app = builder.Build();
await app.RunAsync();