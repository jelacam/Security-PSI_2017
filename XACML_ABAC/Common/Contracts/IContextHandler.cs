using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common.Contracts
{
    [ServiceContract]
    public interface IContextHandler
    {
        [OperationContract]
        DecisionType CheckAccess(Dictionary<string, List<DomainAttribute>> DomainAttributes);

        //[OperationContract]
        //List<DomainAttribute> RequestForEnvironmentAttribute(List<string> AttributeTypes);

    }

    //public interface IContextHandlerCallback
    //{
    //    [OperationContract(IsOneWay = true)]
    //    void ReturnResponse(bool response);
    //}
}
