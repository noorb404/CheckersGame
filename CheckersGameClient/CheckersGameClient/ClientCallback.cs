using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using CheckersGameClient.ServiceReference2;

namespace CheckersGameClient
{


    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class ClientCallback : ICheckersServiceCallback
    {
        public static Action<string[]> updateUsers = null;
        public static Action<string> displayChallenge = null;
        public static Action<double, double> newStep = null;
        public static Action<string> updateInfo = null;
        public static bool Challange = false;


        public void NewStep(double x, double y)
        {
            newStep(x, y);
        }



        public bool SendChallengeToClient(string fromClient)
        {
            displayChallenge(fromClient);
            return Challange;

        }


        public void UpdateProfileInfo(string info)
        {
            updateInfo(info);
        }


        //public event UpdateListDelegate updateUsers;
        public void UpdateClientsList(string[] users)
        {
            updateUsers(users);
        }
        public void UpdateClientsList(IEnumerable<string> users)
        {
            //UpdateClientsList2(users);
        }
        public delegate void SearchDelegate(string[] users);
        public event SearchDelegate srch;
        public void SearchC(string[] users)
        {
            srch(users);
        }


        public void SearchC(IEnumerable<string> op)
        {
            throw new NotImplementedException();
        }

    }
}
