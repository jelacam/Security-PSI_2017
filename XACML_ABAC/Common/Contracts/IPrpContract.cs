using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common.Contracts
{
    [ServiceContract, XmlSerializerFormat]
    public interface IPrpContract
    {
        [OperationContract, XmlSerializerFormat]
        PolicySetType Load();
    }
}