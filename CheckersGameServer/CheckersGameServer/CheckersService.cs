using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CheckersGameServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CheckersService : ICheckersService
    {
        public static SortedDictionary<string, ICheckersServiceCallback> clients= new SortedDictionary<string, ICheckersServiceCallback>();
        public static  SortedDictionary<string, string> OnlineDous= new SortedDictionary<string, string>();
        static DataClasses1DataContext dc;
        

        public CheckersService()
        {
            dc = new DataClasses1DataContext();
        }

        public bool CheckUserName(string userName)
        {
            var s = (from u in dc.Players
                     where u.UserName == userName
                     select u).FirstOrDefault();
            if (s == null) return false;
            return true;
        }
        public bool AddCustomer(string username, string pssword, string FirstName, string LastName, string City)
        {
            if (CheckUserName(username))
                return false;
            dc.GetTable<Player>().InsertOnSubmit(new Player
            {
                UserName = username,
                Password = pssword,
                IsOnline = false,
                IsPlaying = false,
                Games = 0,
                Wins = 0,
                Loses = 0,
                FirstName=FirstName,
                LastName=LastName,
                City=City
            });
            dc.SubmitChanges();
            return true;
        }
        public void setClient(string username)
        {
            ICheckersServiceCallback callback =  OperationContext.Current.GetCallbackChannel<ICheckersServiceCallback>();
            clients.Add(username, callback);
            //var s = getOnlineUsers(username);
            
            //Task.Factory.StartNew(() =>
            //{
              //  callback.NewStep(10);
            //});
            
            foreach (var c in clients.Values)
                c.UpdateClientsList(clients.Keys);
        }
        public bool LogIn(string username, string pssword)
        {
            var s = (from u in dc.Players
                     where u.UserName == username &&
                            u.Password == pssword
                     select u).FirstOrDefault<Player>();
            if (s == null)
                return false;
            s.IsOnline = true;
           
            dc.SubmitChanges();
            return true;

        }
        public IEnumerable<string> getOnlineUsers(string username )
        {
            var s = (from u in dc.Players where u.IsOnline==true select u.UserName);
            return s;

        }
        public void Search(string op, string usr)
        {
            if (op.Equals("USR"))
            {
                List<string> c = (from u in dc.Players
                                  orderby u.UserName
                                  select u.UserName).ToList();
                clients[usr].SearchC(c);
            }
            else if (op.Equals("WIN"))
            {
                var c = (from u in dc.Players
                         orderby u.Wins
                         select u.Wins + u.UserName).ToList();
                clients[usr].SearchC(c);

            }
            else if (op.Equals("LOSE"))
            {
                var c = (from u in dc.Players
                         orderby u.Loses
                         select u.Loses + u.UserName).ToList();
                clients[usr].SearchC(c);

            }
            else if (op.Equals("GAME"))
            {
                var c = (from u in dc.Players
                         orderby u.Games
                         select u.Games + u.UserName).ToList();
                clients[usr].SearchC(c);
            }
        }
        public void UpdateWin(string userName)
        {
            var s = (from u in dc.Players
                     where u.UserName == userName
                     select u).FirstOrDefault<Player>();
            s.Wins++;
            s.Games++;
            dc.SubmitChanges();
        }
        public void UpdateLose(string userName)
        {

            var s = (from u in dc.Players
                     where u.UserName == userName
                     select u).FirstOrDefault<Player>();
            s.Loses++;
            s.Games++;
            dc.SubmitChanges();
        }
        public bool AddStepToTheBoard(double x,double y, string fromClient)
        {
            if (!OnlineDous.ContainsKey(fromClient)) return false;
            if (!clients.ContainsKey(OnlineDous[fromClient])) return false;
            Thread t2 = new Thread(() => clients[OnlineDous[fromClient]].NewStep(x,y));
            t2.Start();
            return true;
        }
        public void outOfChallenge(string fromClient, string toClient)
        {
            OnlineDous.Remove(fromClient);
            OnlineDous.Remove(toClient);
        }
        public bool SendChallenge(string fromClient, string toClient)
        {
            if (fromClient == toClient) return false;
            if (OnlineDous.ContainsKey(toClient)) return false;

            bool answer = false;
            int flag = 0;
            if (clients.ContainsKey(toClient))
            {          
                answer = clients[toClient].SendChallengeToClient(fromClient);
            }

            if (answer == true)
            {
                //foreach (KeyValuePair<string, string> o in OnlineDous)
                //{
                //    if (o.Key.Equals(fromClient) && o.Value.Equals(toClient))
                //        flag = 1;
                //}
                //if (flag == 0)
                {
                    OnlineDous.Add(fromClient, toClient);
                    OnlineDous.Add(toClient, fromClient);

                    var s = (from u in dc.Players where u.UserName == fromClient select u).FirstOrDefault<Player>();
                    s.Games += 1;
                    dc.SubmitChanges();
                    var s2 = (from u in dc.Players where u.UserName == toClient select u).FirstOrDefault<Player>();
                    s2.Games += 1;
                    dc.SubmitChanges();

                }
            }
            return answer;
        }
        private void updateClients()
        {
            foreach (var c in clients.Values)
                c.UpdateClientsList(clients.Keys);
        }
        public void LogOut(string userName)
        {
            clients.Remove(userName);
            Thread updateThread = new Thread(updateClients);
            updateThread.Start();
            var s = (from u in dc.Players where u.UserName == userName select u).FirstOrDefault<Player>();
            s.IsOnline = false;
            dc.SubmitChanges();
        }
        public void ShowInfo(string fromclient, string toclient)
        {
            Player cus = (from c in dc.Players
                            where c.UserName == toclient
                            select c).FirstOrDefault<Player>();

            double rate;
            if (cus.Loses != 0)
            {
                rate = ((double)cus.Wins / (double)cus.Loses);
                int temp = (int)(rate * 100);
                rate = (double)temp / 100.0;
            }
            else
                rate = (int) cus.Loses;
            string s = "User name: " + cus.UserName + "\n" + "Games: " + cus.Games + "\n"
                       + "Wins: " + cus.Wins + "\n" + "Loses: " + cus.Loses + "\n" + "Rate: " +
                       rate;
            Thread t2 = new Thread(() => clients[fromclient].UpdateProfileInfo(s));
            t2.Start();


            return;
        }
    }
}
