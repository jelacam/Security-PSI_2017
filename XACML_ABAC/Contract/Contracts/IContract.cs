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
    [ServiceContract(CallbackContract = typeof(IContractCallback), SessionMode = SessionMode.Required)]
    public interface IContract
    {
        [OperationContract(Action = "view_remaining courses")]
        string ViewRemainingCourses();

        [OperationContract(Action = "edit_remaining courses")]
        string EditRemainingCourses();

        [OperationContract(Action = "register_exam")]
        string RegisterExam();
    }

    public interface IContractCallback
    {
        [OperationContract]
        string RequestClientIpAddress();
    }
}