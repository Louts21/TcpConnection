using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

// dotnet run / dotnet build
public class ExporterProgram
{
    private static Socket sender = null;
    private static NetworkStream networkStream = null;
    private static StreamWriter streamWriter = null;
    private static StreamReader streamReader = null;

    private static int counter = 0;

    public static void Main(string[] args)
    {
        CreateConnection();
        SendData("1", "Hallo Server");
        SendData("2", 2.34d);
        SendData("3", 24);
        // CloseConnection();
    }

    private static void CreateConnection()
    {
        try
        {
            IPHostEntry host = Dns.GetHostEntry("127.0.0.1");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11100);

            Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sender.Connect(localEndPoint);

                Console.WriteLine("Socket connected to {0} ", sender.RemoteEndPoint.ToString());

                networkStream = new NetworkStream(sender);
                streamWriter = new StreamWriter(networkStream);
                streamReader = new StreamReader(networkStream);
            }

            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }

            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }

            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }

        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static void SendData(string packageNumber, string sendableText)
    {
        if (streamWriter != null)
        {
            counter++;
            streamWriter.WriteLine(counter);
            streamWriter.WriteLine(sendableText);
            streamWriter.Flush();

        }

        while (true)
        {
            if (ReplyObtained().Contains(counter.ToString()))
            {
                break;
            }
        }
    }

    private static void SendData(string packageNumber, double sendableData)
    {
        if (streamWriter != null)
        {
            counter++;
            streamWriter.WriteLine(counter);
            streamWriter.WriteLine(sendableData);
            streamWriter.Flush();

        }

        while (true)
        {
            if (ReplyObtained().Contains(counter.ToString()))
            {
                break;
            }
        }
    }

    private static void SendData(string packageNumber, long sendableData)
    {
        if (streamWriter != null)
        {
            counter++;
            streamWriter.WriteLine(counter);
            streamWriter.WriteLine(sendableData);
            streamWriter.Flush();
        }

        while (true)
        {
            if (ReplyObtained().Contains(counter.ToString()))
            {
                break;
            }
        }
    }

    private static string ReplyObtained()
    {
        while (true)
        {
            int length;
            Byte[] bytes = new byte[1024];

            while ((length = networkStream.Read(bytes, 0, bytes.Length)) != 0)
            {
                var incommingData = new byte[length];
                Array.Copy(bytes, 0, incommingData, 0, length);
                string clientMessage = Encoding.ASCII.GetString(incommingData);
                Console.WriteLine("Server says: " + clientMessage);
                return clientMessage;
            }
        }
    }

    private static void CloseConnection()
    {
        try
        {
            Console.WriteLine("Client disconnected!");
            streamWriter.Close();
            networkStream.Close();
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }

        catch (SocketException se)
        {
            Console.WriteLine("SocketException : {0}", se.ToString());
        }

        catch (Exception e)
        {
            Console.WriteLine("Unexpected exception : {0}", e.ToString());
        }

    }
}
