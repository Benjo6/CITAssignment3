using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CJTPClient
{
    internal static class Program
    {
        public static void Main(string[] args)
        {TcpClient client = new TcpClient("localhost", 5000); // Connect to the server

            // Construct a CJTP request
            string requestJson = "{\"Method\":\"update\",\"Path\":\"/example\",\"Date\":\"2023-10-13\",\"Body\":\"Some data\"}";

            NetworkStream stream = client.GetStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.WriteLine(requestJson);
            writer.Flush();

            // Read the response from the server
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string responseJson = reader.ReadLine();

            Console.WriteLine("Response from server:");
            Console.WriteLine(responseJson);

            client.Close();
        }
    }
}