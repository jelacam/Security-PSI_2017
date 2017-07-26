using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Contracts.Contracts
{
    [ServiceContract]
    public interface IPipContract
    {
        [OperationContract]
        DomainAttribute RequestEnvironmentAttribute(string AttrType);
    }
}
