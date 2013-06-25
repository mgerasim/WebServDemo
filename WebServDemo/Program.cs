using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

using System.Threading;
using WebServDemo.Factories;

namespace WebServDemo
{
    public static class Settings
    {
        public static int Port;
        public static int XmlFileStorage;
        public static int SqliteStorage;
    }

    class Program
    {    
        static void Main(string[] args)
        {
            Settings.Port = (int)Helper.GetSettingPort();
            Settings.XmlFileStorage = (int)Helper.GetSettingXmlFile();
            Settings.SqliteStorage = (int)Helper.GetSettingSqlite();

            Console.WriteLine("Config in '{0}'", Helper.GetPath());
            Console.WriteLine("Initialize port on {0}", Settings.Port);
            Console.WriteLine("Used XmlFile Storage  {0}", Settings.XmlFileStorage);
            Console.WriteLine("Used Sqlite Storage  {0}", Settings.SqliteStorage);
                                    
            HttpServer httpServer;
            
            httpServer = new HttpServer(Settings.Port);
            
            Thread thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
            return ;

        }

        static void Client(TcpClient Client)
        {
            // Код простой HTML-странички
            string Html = "<html><body><h1>It works!</h1></body></html>";
            // Необходимые заголовки: ответ сервера, тип и длина содержимого. После двух пустых строк - само содержимое
            string Str = "HTTP/1.1 200 OK\nContent-type: text/html\nContent-Length:" + Html.Length.ToString() + "\n\n" + Html;
            // Приведем строку к виду массива байт
            byte[] Buffer = Encoding.ASCII.GetBytes(Str);
            // Отправим его клиенту
            Client.GetStream().Write(Buffer,  0, Buffer.Length);
            // Закроем соединение
            Client.Close();
        }
        static int CountSpaces(string key)
        {
            return key.Length - key.Replace(" ", string.Empty).Length;
        }

        static string ReadLine(Stream stream)
        {
            var sb = new StringBuilder();
            var buffer = new List<byte>();
            while (true)
            {
                buffer.Add((byte)stream.ReadByte());
                var line = Encoding.ASCII.GetString(buffer.ToArray());
                if (line.EndsWith(Environment.NewLine))
                {
                    return line.Substring(0, line.Length - 2);
                }
            }
        }

        static byte[] GetBigEndianBytes(int value)
        {
            var bytes = 4;
            var buffer = new byte[bytes];
            int num = bytes - 1;
            for (int i = 0; i < bytes; i++)
            {
                buffer[num - i] = (byte)(value & 0xffL);
                value = value >> 8;
            }
            return buffer;
        }
       
    }
}
