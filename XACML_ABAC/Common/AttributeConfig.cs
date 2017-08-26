using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class AttributeConfig
    {
        private static ResourceManager resourceManager = null;
        private static ResourceSet resourceSet = null;
        public static object resourceLock = new object();

        public static ResourceManager ResourceManager
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceManager == null)
                    {
                        resourceManager = new ResourceManager(typeof(ABAC_Attributes).FullName, Assembly.GetExecutingAssembly());
                    }
                    return resourceManager;
                }
            }
        }

        public static ResourceSet ResourceSet
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceSet != null)
                    {
                        resourceSet = ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
                    }

                    return resourceSet;
                }
            }
        }

        public static string GetValue(string rolename)
        {
            return ResourceManager.GetString(rolename);
        }
    }
}