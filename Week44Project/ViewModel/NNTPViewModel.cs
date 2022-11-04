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
        // Property for user name textbox
        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; PropertyIsChanged(); }
        }

        // Property for password textbox
        private string pass = "";

        public string Pass
        {
            get { return pass; }
            set { pass = value; PropertyIsChanged(); }
        }

        // Property for the actual artikel body
        private string articleBody;

        public string ArticleBody
        {
            get { return articleBody; }
            set { articleBody = value; PropertyIsChanged(); }
        }

        // Property for ListView Group
        private ObservableCollection<string> groupeList = new ObservableCollection<string>();

        public ObservableCollection<string> GroupeList
        {
            get { return groupeList; }
            set { groupeList = value; PropertyIsChanged(); }
        }

        // Property for ListView Artikel
        private ObservableCollection<ArticlesHolder> artikelList = new ObservableCollection<ArticlesHolder>();

        public ObservableCollection<ArticlesHolder> ArtikelList
        {
            get { return artikelList; }
            set { artikelList = value;PropertyIsChanged(); }
        }

        // Property for status TextBlock
        private string connectStatus;

        public string ConnectStatus
        {
            get { return connectStatus; }
            set { connectStatus = value; PropertyIsChanged(); }
        }

        // Property for Group select item ListView
        private string articles;

        public string Articles
        {
            get { return articles; }
            set { articles = value; 
                PropertyIsChanged();
                retriveArticlesNameThread();
            }
        }

        // Property for Artikel select item ListView
        private ArticlesHolder articleHolder;

        public ArticlesHolder ArticleHolder
        {
            get { return articleHolder; }
            set
            {
                articleHolder = value;
                PropertyIsChanged();
                retriveArticlesThread();
            }
        }


        //command to login
        public AddCommand login { get; set; }

        // global object for the NNTPService
        private NNTPService Service;

        // constrtctor makes a object of the NNTP service and a command for login
        public NNTPViewModel()
        {
            this.Service = new NNTPService();
            login = new AddCommand(Login);
        }

        /**
         * cassle the connect method and verify the connection is made
         */
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

        // makes a thred to run the getGroup method
        private void getGroupThread()
        {
            Thread thread = new Thread(getGroup);
            thread.Start();
        }

        /*
         * calles the Service.getGroups method and gets a list
         * then check it got somting back
         * and the opdates the coresponding property
         */
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

        // makes a thred to run the retriveArticlesName method
        private void retriveArticlesNameThread() 
        {
            Thread thread = new Thread(retriveArticlesName);
            thread.Start();
        }

        /*
         * calles the Service.getArticlesName method and gets a list
         * and then opdates the property
         */
        private void retriveArticlesName() 
        {
            List<ArticlesHolder> articels = Service.getArticlesName(Articles);
            ArtikelList = new ObservableCollection<ArticlesHolder>(articels);
        }

        // makes a thred to run the retriveArticles method
        private void retriveArticlesThread() 
        {
            Thread thread = new Thread(retriveArticles);
            thread.Start();
        }

        /*
        * calles the Service.getArticles method and gets a list
        * and then opdates the property
        */
        private void retriveArticles() 
        {
            string articleNumber = ArticleHolder.articlesNumber;
            string selectedArticle = Service.getArticles(articleNumber);
            selectedArticle.TrimStart();
            ArticleBody = selectedArticle;
        }

    }
}
