using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace InputSocketClient
{
    public class InputSocketClient
    {
        public static void Main()
        {
            Console.WriteLine("入力してください");
            string sendMsg = Console.ReadLine();

            if (sendMsg == null || sendMsg.Length == 0)
            {
                return;
            }

            string ipOrHost = "127.0.0.1";
            int port = 2001;


            TcpClient tcp =
              new TcpClient(ipOrHost, port);
            Console.WriteLine("サーバー({0}:{1})と接続しました({2}:{3})。",
                ((IPEndPoint)tcp.Client.RemoteEndPoint).Address,
                ((IPEndPoint)tcp.Client.RemoteEndPoint).Port,
                ((IPEndPoint)tcp.Client.LocalEndPoint).Address,
                ((IPEndPoint)tcp.Client.LocalEndPoint).Port);


            NetworkStream ns = tcp.GetStream();

            ns.ReadTimeout = 10000;
            ns.WriteTimeout = 10000;


            Encoding enc = Encoding.UTF8;
            byte[] sendBytes = enc.GetBytes(sendMsg + '\n');

            ns.Write(sendBytes, 0, sendBytes.Length);
            Console.WriteLine(sendMsg);


            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            byte[] resBytes = new byte[256];
            int resSize = 0;
            do
            {
                resSize = ns.Read(resBytes, 0, resBytes.Length);

                if (sendMsg.Equals("exit"))
                {
                    Console.WriteLine("サーバーが切断しました。");
                    break;
                }

                ms.Write(resBytes, 0, resSize);

            } while (ns.DataAvailable || resBytes[resSize - 1] != '\n');

            string resMsg = enc.GetString(ms.GetBuffer(), 0, (int)ms.Length);
            ms.Close();

            resMsg = resMsg.TrimEnd('\n');
            Console.WriteLine(resMsg);

            ns.Close();
            tcp.Close();
            Console.WriteLine("切断しました。");

            Console.ReadLine();
        }
    }
}
