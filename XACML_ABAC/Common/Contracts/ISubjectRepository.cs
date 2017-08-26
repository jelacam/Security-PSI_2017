using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    [ServiceContract]
    public interface ISubjectRepository
    {
        [OperationContract]
        bool IsAuthenticated();

        [OperationContract]
        void SetLocation(string userId, string location);

        [OperationContract]
        string AuthenticatedUserId();
    }
}