using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ClientServer.Protocol;
using Newtonsoft.Json;
using Serilog;

namespace ClientServer.Tcp
{
    /// <summary>
    /// Represents a CJTP (Custom JSON Transfer Protocol) server.
    /// </summary>
    public class CJTPServer : IDisposable
    {
        private const int BufferSize = 1024;
        private TcpListener _tcpListener;
        private readonly object _lock;

        /// <summary>
        /// Initializes a new instance of the CJTPServer class with the specified port.
        /// </summary>
        /// <param name="port">The port on which the server will listen for incoming connections.</param>
        public CJTPServer(int port)
        {
            _lock = new object();
            _tcpListener = new TcpListener(IPAddress.Any, port);
        }

        /// <summary>
        /// Starts the CJTP server and begins listening for incoming connections.
        /// </summary>
        public void Start()
        {
            _tcpListener.Start();
            // Begin accepting a client connection asynchronously.
            _tcpListener.BeginAcceptTcpClient(HandleClient, null);
        }

        /// <summary>
        /// Asynchronous callback for handling client connections.
        /// </summary>
        /// <param name="asyncResult">The result of the asynchronous operation.</param>
        private void HandleClient(IAsyncResult asyncResult)
        {
            lock (_lock) // Ensure thread safety when handling clients
            {
                var client = _tcpListener.EndAcceptTcpClient(asyncResult);
                // Continue to accept new client connections.
                _tcpListener.BeginAcceptTcpClient(HandleClient, null);

                using (NetworkStream stream = client.GetStream())
                {
                    var requestBytes = new byte[BufferSize];
                    stream.ReadTimeout = 5000;

                    try
                    {
                        var bytesRead = stream.Read(requestBytes, 0, requestBytes.Length);
                        var requestJson = Encoding.UTF8.GetString(requestBytes, 0, bytesRead);
                        var request = JsonConvert.DeserializeObject<CJTPRequest>(requestJson);

                        var response = ProcessRequest(request);
                        var responseJson = JsonConvert.SerializeObject(response);
                        var responseBytes = Encoding.UTF8.GetBytes(responseJson);

                        // Send the response to the client.
                        stream.Write(responseBytes, 0, responseBytes.Length);
                    }
                    catch (IOException ex)
                    {
                        Log.Error("IO Error: {0}", ex.Message);
                    }
                    catch (JsonException ex)
                    {
                        Log.Error("JSON Error: {0}", ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error: {0}", ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Disposes of the CJTP server by stopping the TCP listener.
        /// </summary>
        public void Dispose()
        {
            _tcpListener?.Stop();
            _tcpListener = null;
        }

        /// <summary>
        /// Process a CJTP request and generate a response.
        /// </summary>
        /// <param name="request">The CJTPRequest to be processed.</param>
        /// <returns>A CJTPResponse representing the response to the request.</returns>
        private CJTPResponse ProcessRequest(CJTPRequest request)
        {
            var response = new CJTPResponse();

            if (string.IsNullOrEmpty(request.Method))
            {
                response.Status = StatusCodes.MissingMethod;
                response.Body = "missing method";
                return response;
            }

            if (string.IsNullOrEmpty(request.Path) && request.Method != "echo")
            {
                response.Status = StatusCodes.MissingPath;
                response.Body = "missing path";
                return response;
            }

            if (request.Date is null)
            {
                response.Status = StatusCodes.MissingDate;
                response.Body = "missing date";
                return response;
            }

            switch (request.Method)
            {
                case "create":
                    response.Status = StatusCodes.Created;
                    response.Body = "Created";
                    break;

                case "read":
                    response.Status = StatusCodes.Ok;
                    response.Body = request.Path;
                    break;

                case "update":
                    response.Status = StatusCodes.Updated;
                    response.Body = "Updated";
                    break;

                case "delete":
                    response.Status = StatusCodes.Ok;
                    response.Body = "Ok";
                    break;

                case "echo":
                    response.Status = StatusCodes.Ok;
                    response.Body = request.Body;
                    break;

                default:
                    response.Status = StatusCodes.IllegalMethod;
                    response.Body = "illegal method";
                    return response;
            }

            return response;
        }
    }
}