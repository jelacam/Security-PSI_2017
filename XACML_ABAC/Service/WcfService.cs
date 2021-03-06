﻿using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.IdentityModel.Services;
using System.Security.Permissions;
using System.IdentityModel.Claims;

namespace Service
{
    //[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,
    //                 InstanceContextMode = InstanceContextMode.PerCall,
    //                 UseSynchronizationContext = false)]
    public class WcfService : IContract
    {
        public string EditRemainingCourses()
        {
            Console.WriteLine("Test method for access denied. [Student try to edit remaining courses]");

            return "Edit student remaining courses...";
        }

        public string ViewRemainingCourses()
        {
            Console.WriteLine("Test method for access permit. [Student try to view remaining courses]");
            return "Student remaining courses...";
        }

        public string RegisterExam()
        {
            Console.WriteLine("Test method for exam registration. [Student try to register an exam]");
            return "Student exam registration...";
        }
    }
}