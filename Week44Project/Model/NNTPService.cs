using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
            if (socket != null) 
            { 
            socket.Close();
            }
            if (ns != null)
            {
                ns.Close();
            }
            if (reader != null)
            {
                reader.Close();
            }
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
            byte[] name = Encoding.UTF8.GetBytes("AUTHINFO USER "+userName+"\n");
            byte[] password = Encoding.UTF8.GetBytes("AUTHINFO PASS "+pass + "\n");
            string returnCheck = "";
            try
            {
                ns.Write(name, 0, name.Length);

                returnCheck = reader.ReadLine();
                Console.WriteLine(returnCheck);
                if (returnCheck.StartsWith("381"))
                {
                    ns.Write(password, 0, password.Length);
                    returnCheck = reader.ReadLine();
                    Console.WriteLine(returnCheck);
                    if (returnCheck.StartsWith("250") || returnCheck.StartsWith("281"))
                    {
                        loginConfirm = "Login Accepted";
                        return loginConfirm;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return loginConfirm;
        }

        public List<string> getGroups()
        {
            ns.Flush();
            List<string> groups = new List<string>();
            byte[] group = Encoding.UTF8.GetBytes("LIST" + "\n");
            try
            {
                ns.Write(group, 0, group.Length);
                bool flag = true;
                while (flag) {
                    string newsGroups = reader.ReadLine();
                    Console.WriteLine(newsGroups);
                    if (newsGroups.Equals("."))
                    {
                        flag = false;
                        break;
                    }
                    string[] newsGroupsList = Regex.Split(newsGroups, @"\s+", RegexOptions.IgnorePatternWhitespace);
                    groups.Add(newsGroupsList[0]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return groups;
        }
    }
}
