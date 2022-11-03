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

        public void CloseConnection() 
        { 
            socket.Close();
            ns.Close();
            reader.Close();
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

                string test = reader.ReadLine();
                Console.WriteLine(test);

                connectConfirm = true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public string LoginToSever(string userName, string pass)
        {
            string loginConfirm = "Login Failed";
            byte[] name = Encoding.UTF8.GetBytes("AUTHINFO USER "+userName);
            byte[] password = Encoding.UTF8.GetBytes("AUTHINFO PASS "+pass);
            string returnCheck = "";
            try
            {
                ns.Write(name, 0, name.Length);

                returnCheck = reader.ReadLine(); 

                if (returnCheck.StartsWith("381"))
                {
                    ns.Write(password, 0, password.Length);
                    returnCheck = reader.ReadLine();
                    if (returnCheck.StartsWith("250") || returnCheck.StartsWith("281"))
                    {
                        loginConfirm = "Login Accepted";
                        return loginConfirm;
                    }
                }

                CloseConnection();
                ConnectNTTP();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return loginConfirm;
        }
    }
}
