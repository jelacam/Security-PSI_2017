using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common.Contracts
{
    [ServiceContract]
    public interface IPipContract
    {
        [OperationContract]
        DomainAttribute RequestEnvironmentAttribute(string AttrType);

        [OperationContract]
        List<DomainAttribute> RequestSubjectAttributes(string AttrType, string SubjectId);
    }
}