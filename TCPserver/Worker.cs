using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BookTest;
using Newtonsoft.Json;

namespace TCPserver
{
    class Worker
    {
        private static List<Bog> boglist = new List<Bog>()
        {
            new Bog(){Forfatter = "Nikolai", Titel = "Første bog", Sidetal = 10, Isbn = "1234567890123"}
        };



        public Worker()
        {
        }

        public void Start()
        {
            TcpListener server = new TcpListener(IPAddress.Loopback, 4646);
            server.Start();

            while (true)
            {
                TcpClient socket = server.AcceptTcpClient();


                Task.Run(

                    () =>
                    {
                        TcpClient tmpsocket = socket;
                        DoClient(tmpsocket);
                    }
                );

            }


        }

        private void DoClient(TcpClient socket)
        {
            using (StreamReader sr = new StreamReader(socket.GetStream()))
            using (StreamWriter sw = new StreamWriter(socket.GetStream()))
            {
                string operation = sr.ReadLine();
                string data = sr.ReadLine();
                switch (operation)
                {
                    case "HentAlle":
                        string json = JsonConvert.SerializeObject(boglist);
                        sw.WriteLine(json);
                        break;
                    case "Hent":
                        Bog b = boglist.FirstOrDefault(bog => bog.Isbn == data);
                        json = JsonConvert.SerializeObject(b);
                        sw.WriteLine(json);
                        break;
                    case "Gem":
                        boglist.Add(JsonConvert.DeserializeObject<Bog>(data));
                        break;
                }

                
                sw.Flush();
            }

            socket?.Close();
        }
    }

}
