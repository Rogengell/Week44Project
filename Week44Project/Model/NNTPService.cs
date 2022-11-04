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

        // Closses all conections
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

        // Connects to the server
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

        // Writes to sever and veryfie the login
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

        // Writes to the sever and gets the list of news groups
        public List<string> getGroups()
        {
            List<string> groups = new List<string>();
            byte[] group = Encoding.UTF8.GetBytes("LIST" + "\n");
            try
            {
                ns.Write(group, 0, group.Length);
                Console.WriteLine(reader.ReadLine());
                bool flag = true;
                while (flag) {
                    string newsGroups = reader.ReadLine();
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

        // Writes to the sever and gets all article names and id numbers
        public List<ArticlesHolder> getArticlesName(string newsGroups) 
        {
            List<string> articlesNumber = new List<string>();
            List<string> articles = new List<string>();
            List<ArticlesHolder> retunrList = new List<ArticlesHolder>();
            
            byte[] group = Encoding.UTF8.GetBytes("LISTGROUP "+ newsGroups + "\n");
            
            try
            {
                ns.Write(group, 0, group.Length);
                string confirm = reader.ReadLine();
                Console.WriteLine(confirm);
                if (confirm.StartsWith("211")) 
                {
                    bool flag1 = true;
                    while (flag1) 
                    {
                        string number = reader.ReadLine();
                        if (number.StartsWith("."))
                        {
                            flag1=false;
                            break;
                        }
                        articlesNumber.Add(number);
                    }

                    foreach (string s in articlesNumber)
                    {
                        byte[] HEAD = Encoding.UTF8.GetBytes("HEAD " + s + "\n");
                        ns.Write(HEAD, 0, HEAD.Length);
                        bool flag2 = true;
                        while (flag2)
                        { 
                            string subject = reader.ReadLine();

                            if (subject.StartsWith("."))
                            {
                                flag2 = false;
                            }

                            if (subject.StartsWith("Subject:") || subject.StartsWith("subject:"))
                            {
                                string name = Regex.Replace(subject, "Subject: | subject:", "");
                                name.TrimStart();
                                articles.Add(name);
                                retunrList.Add(new ArticlesHolder(s,name));
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return retunrList;
        }

        // Writes to the sever and gets the actual article
        public string getArticles(string articleNumber) 
        {
            string fullArticle = "";
            byte[] commmand = Encoding.UTF8.GetBytes("BODY " + articleNumber + "\n");

            try
            {
                ns.Write(commmand, 0, commmand.Length);
                string confirm = reader.ReadLine();
                Console.WriteLine(confirm);
                if (confirm.StartsWith("222")) 
                {
                    bool flag = true;

                    while (flag)
                    {
                        string gottenLine = reader.ReadLine();

                        if (gottenLine.StartsWith(".")) 
                        {
                            break;
                        }
                        fullArticle += gottenLine+"\n";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return fullArticle;
        }
    }
}
