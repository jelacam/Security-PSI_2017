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
            string value = DateTime.UtcNow.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();

            Console.WriteLine("PIP - Request for environment attribute - current time: {0}", value.ToString());

            DomainAttribute CurrentTime = new DomainAttribute();
            CurrentTime.Value = value;
            CurrentTime.AttributeId = "current-time";
            CurrentTime.DataType = "time";
            CurrentTime.Category = "environment";

            return CurrentTime;
        }
    }
}