using Microsoft.Xrm.Sdk;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Tooling.Connector;

namespace Logistics.Dynamics365.Plugins.Conexoes
{
    public class DynamicsConnection 
    {
        public IOrganizationService Service { get; set; }

        public DynamicsConnection(string environmentName)
        {
            string connectionString = ConfigurationManager.ConnectionStrings[environmentName].ConnectionString;

            CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);

            Service = crmServiceClient;
        }

    }
}
