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
            string value = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();

            DomainAttribute CurrentTime = new DomainAttribute();
            CurrentTime.Value = value;

            return CurrentTime;
        }
    }
}
