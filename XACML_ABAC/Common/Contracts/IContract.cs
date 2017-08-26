using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.IdentityModel.Services;
using System.Security.Permissions;

namespace Common
{
    [ServiceContract]
    public interface IContract
    {
        [OperationContract(Action = "view_rc")]
        string ViewRemainingCourses();

        [OperationContract(Action = "edit_rc")]
        string EditRemainingCourses();

        [OperationContract(Action = "register_ex")]
        string RegisterExam();
    }
}