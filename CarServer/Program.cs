using CarLibrary;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarServer
{
    //Remember to add the CarLibrary DLL as a project reference
    class Program
    {
        static void Main(string[] args)
        {
            //Writes to the console what is running
            Console.WriteLine("Car server");

            //Creates a Listener that listens on all network adapters on port 10002
            TcpListener listener = new TcpListener(IPAddress.Any, 10002);
            //Starts the listener
            listener.Start();

            //Handles more clients
            while (true)
            {
                //Waits for a client to connect
                TcpClient socket = listener.AcceptTcpClient();
                //Makes the server concurrent
                //And sends the client to the method HandleClient
                Task.Run(() => HandleClient(socket));
            }
        }

        private static void HandleClient(TcpClient socket)
        {
            //Gets the stream object from the socket. The stream object is able to recieve and send data
            NetworkStream ns = socket.GetStream();
            //The StreamReader is an easier way to read data from a Stream, it uses the NetworkStream
            StreamReader reader = new StreamReader(ns);

            //no writer here, as it doesn't send data back to the client

            //Reads a line from the client, here it expects the entire JSON in one line
            string message = reader.ReadLine();

            //Here it converts the message the client send to a Car object
            //if the client sends something wrong, properties won't be set
            Car receivedCar = JsonSerializer.Deserialize<Car>(message);

            //writes the 3 properties to the console, one property per line
            Console.WriteLine("Car Model: " + receivedCar.Model);
            Console.WriteLine("Car Color: " + receivedCar.Color);
            Console.WriteLine("Car RegistrationNumber: " + receivedCar.RegistrationNumber);

            //closes the socket, as it doesn't expect anything more from the client
            socket.Close();

            //JSON example to test with
            //{"Model":"BMW","Color":"Black","RegistrationNumber":"AB12345"}
        }
    }
}
