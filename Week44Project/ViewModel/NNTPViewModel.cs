using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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

        private ObservableCollection<string> artikelList = new ObservableCollection<string>();

        public ObservableCollection<string> ArtikelList
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
            else
            {
                ConnectStatus = "Sever Ready";
            }
        }

    }
}
