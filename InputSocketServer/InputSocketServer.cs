﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace InputSocketServer
{
    public class InputSocketServer
    {
        public static void Main()
        {
            string ipString = "127.0.0.1";

            IPAddress ipAdd = IPAddress.Parse(ipString);

            int port = 2001;

            TcpListener listener = new TcpListener(ipAdd, port);

            listener.Start();

            Console.WriteLine("Listenを開始しました({0}:{1})。",
                ((IPEndPoint)listener.LocalEndpoint).Address,
                ((IPEndPoint)listener.LocalEndpoint).Port);


            // 接続要求があったら受け入れる
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("クライアント({0}:{1})と接続しました。",
                ((IPEndPoint)client.Client.RemoteEndPoint).Address,
                ((IPEndPoint)client.Client.RemoteEndPoint).Port);

            // NetworkStreamを取得データの流れ
            NetworkStream ns = client.GetStream();

            ns.ReadTimeout = 10000;
            ns.WriteTimeout = 10000;

            Encoding enc = Encoding.UTF8;
            bool disconnected = false;
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            byte[] resBytes = new byte[256];
            int resSize = 0;


            do
            {
                //データの一部を受信する
                resSize = ns.Read(resBytes, 0, resBytes.Length);
                //Readが0を返した時はクライアントが切断したと判断
                if (resSize == 0)
                {
                    disconnected = true;
                    Console.WriteLine("クライアントが切断しました。");
                    break;
                }
                //受信したデータを蓄積する
                ms.Write(resBytes, 0, resSize);
                //まだ読み取れるデータがあるか、データの最後が\nでない時は、
                // 受信を続ける

            } while (ns.DataAvailable || resBytes[resSize - 1] != '\n');

            //受信したデータを文字列に変換
            string resMsg = enc.GetString(ms.GetBuffer(), 0, (int)ms.Length);
            ms.Close();

            resMsg = resMsg.TrimEnd('\n');
            Console.WriteLine(resMsg);

            if (!disconnected)
            {
                //クライアントにデータを送信する
                //クライアントに送信する文字列を作成
                string sendMsg = resMsg.Length.ToString();

                //文字列をByte型配列に変換
                byte[] sendBytes = enc.GetBytes(sendMsg + '\n');

                //データを送信する
                ns.Write(sendBytes, 0, sendBytes.Length);
                Console.WriteLine(sendMsg);
            }

            ns.Close();
            client.Close();
            Console.WriteLine("クライアントとの接続を閉じました。");

            listener.Stop();
            Console.WriteLine("Listenerを閉じました。");

            Console.ReadLine();
        }

    }
}
