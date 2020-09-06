using System.Collections.Generic;
using System.ServiceModel;

namespace CheckersGameServer
{
    public interface ICheckersServiceCallback
    {

        [OperationContract(IsOneWay = true)]
        void UpdateClientsList(IEnumerable<string> users);

        [OperationContract(IsOneWay = false)]
        bool SendChallengeToClient(string fromClient);

        [OperationContract(IsOneWay = true)]
        void NewStep(double x,double y);

        [OperationContract(IsOneWay = true)]
        void UpdateProfileInfo(string username);

        [OperationContract(IsOneWay = true)]
        void SearchC(IEnumerable<string> op);
    }
}