using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Contracts.Contracts
{
    [ServiceContract]    
    public interface IPapContract
    {
        [OperationContract]
        PolicyType LoadPolicy();
    }
}
