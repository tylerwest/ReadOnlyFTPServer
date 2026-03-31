# Read-Only FTP Server

This is a toy implementation of an FTP server built with .NET. It implements most of the FTP specification, but only allows for read operations.

## Features

*   Full FTP navigation with support for CWD, CDUP, and PWD.
*   File listings and metadata via LIST, SIZE, and MDTM.
*   File retrieval via RETR with support for binary and ASCII modes.
*   Passive mode and extended passive mode support with PASV and EPSV.
*   Write operations like STOR, DELE, and MKD are disabled.

## Stack

*   .NET 10.0 Generic Host with dependency injection.
*   Async support for all operations.
*   Simple JSON-based configuration for users and server settings.

## Getting Started

### Local Development

Run the server using the .NET CLI:

dotnet run --project ReadOnlyFTPServer

By default, the server listens on port 2121.

### Configuration

The server is configured via three JSON files:

*   appsettings.json: Application settings.
*   server.json: Port and root path settings.
*   users.json: User credentials.

### Docker

Build and run using Docker:

docker build -t readonlyftpserver .
docker run -p 2121:2121 -p 40000-40100:40000-40100 readonlyftpserver

When running in a container, you must expose both the command port and the range of passive ports. By default, the server uses ports 40000 to 40100 for passive data connections.

#### Docker Compose

Alternatively, use the provided docker-compose configuration:

```yaml
services:
  readonlyftpserver:
    build:
      context: .
      dockerfile: ReadOnlyFTPServer/Dockerfile
    ports:
      - "2121:2121"
      - "40000-40100:40000-40100"
    volumes:
      - "/ftp-root:/ftp-root"
```
