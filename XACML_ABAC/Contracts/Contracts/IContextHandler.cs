﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Contracts.Contracts
{
    [ServiceContract]
    public interface IContextHandler
    {
        [OperationContract]
        DecisionType CheckAccess(Dictionary<string, List<DomainAttribute>> DomainAttributes);

    }

    public interface IContextHandlerCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReturnResponse(bool response);
    }
}
