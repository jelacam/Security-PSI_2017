using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.IdentityModel.Services;
using System.Security.Permissions;

namespace Contracts
{
    [ServiceContract]
    public interface IContract
    {
        [OperationContract(Action = "view_remaining courses")]
        string AccessPermit();

        [OperationContract(Action = "edit_remaining courses")]
        string AccessDenied();

        [OperationContract(Action = "register_exam")]
        string ExamRegistration();
    }
}