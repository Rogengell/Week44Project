using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Week44Project.Model
{
    internal class NNTPService
    {
        public TcpClient socket { get; set; }
        public NetworkStream ns { get; set; }
        public StreamReader reader { get; set; }

        public bool connectConfirm { get; set; }

        public NNTPService()
        {
            socket = null;
            ns = null;
            reader = null;
        }
        public void ConnectNTTP()
        {
            String serverName = "news.dotsrc.org";
            int serverPort = 119;
            connectConfirm = false;
            try
            {
                socket = new TcpClient(serverName, serverPort);

                ns = socket.GetStream();

                reader = new StreamReader(ns, Encoding.UTF8);

                Console.WriteLine(reader.ReadLine());
                ns.Flush();
                connectConfirm = true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                socket.Close();
                ns.Close();
                reader.Close();
            }   
        }

        public void LoginToSever(string userName,string pass) 
        { 
            
        }
    }
}
