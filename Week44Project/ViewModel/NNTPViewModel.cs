using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Week44Project.Model;

namespace Week44Project.ViewModel
{
    internal class NNTPViewModel : ViewModelBase
    {
        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; PropertyIsChanged(); }
        }

        private string pass = "";

        public string Pass
        {
            get { return pass; }
            set { pass = value; PropertyIsChanged(); }
        }

        private ObservableCollection<string> groupeList = new ObservableCollection<string>();

        public ObservableCollection<string> GroupeList
        {
            get { return groupeList; }
            set { groupeList = value; PropertyIsChanged(); }
        }

        private ObservableCollection<ArticlesHolder> artikelList = new ObservableCollection<ArticlesHolder>();

        public ObservableCollection<ArticlesHolder> ArtikelList
        {
            get { return artikelList; }
            set { artikelList = value;PropertyIsChanged(); }
        }


        private string connectStatus;

        public string ConnectStatus
        {
            get { return connectStatus; }
            set { connectStatus = value; PropertyIsChanged(); }
        }

        private string articles;

        public string Articles
        {
            get { return articles; }
            set { articles = value; 
                PropertyIsChanged();
                retriveArticlesThread();
            }
        }

        private ArticlesHolder test;

        public ArticlesHolder Test
        {
            get { return test; }
            set
            {
                test = value;
                PropertyIsChanged();
                
            }
        }



        public AddCommand login { get; set; }

        private NNTPService Service;

        public NNTPViewModel()
        {
            this.Service = new NNTPService();
            login = new AddCommand(Login);
            Name = "niel4921@easv365.dk";
            Pass = "cd2bc4";
        }

        public void Login(object parameter)
        {
            Service.CloseConnection();
            Service.ConnectNTTP();
            if (Service.connectConfirm == true)
            {
                ConnectStatus = Service.LoginToSever(name, Pass);
            }
            else
            {
                ConnectStatus = "Sever Ready";
            }
            getGroupThread();
        }

        private void getGroupThread()
        {
            Thread thread = new Thread(getGroup);
            thread.Start();
        }

        private void getGroup()
        {
            List<string> list = Service.getGroups();

            if (list.Count == 0)
            {
                ConnectStatus = "Groups not found on sever";
            }
            else
            {
                GroupeList = new ObservableCollection<string>(list);
            }
        }

        private void retriveArticlesThread() 
        {
            Thread thread = new Thread(retriveArticles);
            thread.Start();
        }

        private void retriveArticles() 
        {
            List<ArticlesHolder> articels = Service.getArticles(Articles);
            ArtikelList = new ObservableCollection<ArticlesHolder>(articels);
        }



    }
}
