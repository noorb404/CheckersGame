using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


// Create a client.  

//CheckersServiceClient client = new CheckersServiceClient(instanceContext);
//InstanceContext instanceContext = new InstanceContext(new ClientCallback());

// Create a client.  
//CheckersServiceClient client = new CheckersServiceClient(instanceContext);

namespace CheckersGameServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ICheckersServiceCallback))]
    public interface ICheckersService
    {
        [OperationContract]
        bool AddCustomer(string username, string pssword, string FirstName, string LastName, string City);
        [OperationContract]
        void setClient(string username);
        [OperationContract]
        bool LogIn(string username, string pssword);
        [OperationContract]
        IEnumerable<string> getOnlineUsers(string username);
        [OperationContract]
        void LogOut(string userName);
        [OperationContract]
        void outOfChallenge(string fromClient, string toClient);
        [OperationContract]
        void ShowInfo(string from, string to);
        [OperationContract]
        bool CheckUserName(string userName);
        [OperationContract]
        bool AddStepToTheBoard(double x,double y, string fromClient);
        [OperationContract]
        bool SendChallenge(string fromClient, string toClient);
        [OperationContract]
        void UpdateWin(string userName);
        [OperationContract]
        void UpdateLose(string userName);
        [OperationContract]
        void Search(string op, string usr);
    }
}
