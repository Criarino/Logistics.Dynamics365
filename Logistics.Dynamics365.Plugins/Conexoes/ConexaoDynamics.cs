using Microsoft.Xrm.Sdk;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Tooling.Connector;
using System.Security.Policy;

namespace Logistics.Dynamics365.Plugins.Conexoes
{
    public class ConexaoDynamics
    {
        public IOrganizationService Service { get; set; }

        public ConexaoDynamics()
        {

            var user = "admin@Logistics573.onmicrosoft.com";
            var password = "$enha123";
            var url = "https://org4847cb57.crm2.dynamics.com";

            CrmServiceClient crmServiceClient = new CrmServiceClient(
                "AuthType=Office365; " +
                $"Username={user};" +
                $"Password={password};" +
                $"Url={url};"
                );

            Service = crmServiceClient.OrganizationWebProxyClient;
        }

    }
}
