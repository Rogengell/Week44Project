using System;
using System.Collections.Generic;
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
        }

        public void Login(object parameter)
        {
            Service.ConnectNTTP();
            if (Service.connectConfirm == true)
            {
                
            }
            else
            {
                ConnectStatus = "Sever Ready";
            }
        }

        




    }
}
