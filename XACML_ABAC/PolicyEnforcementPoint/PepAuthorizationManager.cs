using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PolicyEnforcementPoint
{
    public class PepAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            // service binding i adress
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:8000/PdpService";

            DecisionType decision;
           
            using (PdpProxy proxy = new PdpProxy(binding, new EndpointAddress(new Uri(address))))
            {
                decision = proxy.Evaluate();
            }

            Console.WriteLine("\n" + decision.ToString());


            if (decision == DecisionType.Permit)
            {
                return true;
            }
            else
            {
                return false;
            }

            //ResponseType response = new ResponseType();
            //ResultType result = new ResultType();
            //result.Decision = decision;
            //response.Result = new ResultType[1];
            //response.Result[0] = result;




            //Console.ReadKey();
        }
    }
}
