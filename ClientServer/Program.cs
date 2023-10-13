using System;
using System.Threading;
using Serilog;
using ClientServer.Protocol;
using ClientServer.Tcp;

namespace Server
{
    internal class Program
    {
        private static CJTPServer _cjtpServer;
        private static CJTPClient _cjtpClient;
    
        public static void Main(string[] args)
        {
            ConfigureLogging();

            using (_cjtpServer = new CJTPServer(5000))
            {
                StartServer();
                using (_cjtpClient = new CJTPClient("127.0.0.1", 5000))
                {

                    SendRequest();

                    // Sleep to keep the server running for demonstration purposes.
                    Thread.Sleep(60000);
                }
            }
        
            Log.CloseAndFlush();
        }

        private static void ConfigureLogging()
        {
            // Configure logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
        
            Log.Information("Logging configured");
        }

        private static void StartServer()
        {
            // Start the server on a separate thread.
            new Thread(_cjtpServer.Start).Start();
        
            Log.Information("Server started");
        }

        private static void SendRequest()
        {
            // Use the client to send a request to the server.
            var request = new CJTPRequest
            {
                Method = "echo",
                Path = "/example/resource",
                Date = DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
                Body = "Hello, CJTP Server!"
            };

            var response = _cjtpClient.SendRequest(request);

            Log.Information("Response Status: {Status}", response.Status);
            Log.Information("Response Body: {Body}", response.Body);
        }
    }}