using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Contracts
{
    [ServiceContract]
    public interface IAuthentication
    {
        [OperationContract]
        bool IsAuthenticated();

        [OperationContract]
        void SetLocation(string userId, string location);

        [OperationContract]
        string AuthenticatedUserId();
    }
}