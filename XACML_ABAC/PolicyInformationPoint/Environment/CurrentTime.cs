using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;

namespace PolicyInformationPoint.Environment
{
    public class CurrentTime : EnvironmentAttributes
    {
        public override DomainAttribute RequestForEnvironmentAttributes()
        {
            Console.WriteLine("PIP - Request for environment attribute - current time");
            string value = DateTime.UtcNow.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();

            DomainAttribute CurrentTime = new DomainAttribute();
            CurrentTime.Value = value;
            CurrentTime.AttributeId = "current-time";
            CurrentTime.DataType = "time";
            CurrentTime.Category = "environment";

            return CurrentTime;
        }
    }
}