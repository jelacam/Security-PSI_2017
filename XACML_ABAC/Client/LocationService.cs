using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant,
                      UseSynchronizationContext = false)]
    public class LocationService : IContractCallback
    {
        public string RequestClientIpAddress()
        {
            return "test";
        }
    }
}