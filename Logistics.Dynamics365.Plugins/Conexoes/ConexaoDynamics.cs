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

        public ConexaoDynamics(string environmentName)
        {
            //string teste = "";
            //string connectionString = ConfigurationManager.ConnectionStrings[environmentName].ConnectionString;

            //string connectionString = "AuthType=Office365;Username=admin@Logistics573.onmicrosoft.com;Password=$enha123;Url=https://org4847cb57.crm2.dynamics.com;AppId=cac798d8-1560-4b66-8004-efb6c3058f06;";

            //CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);

            var user = "guiviera@Xmen97.onmicrosoft.com";
            var password = "Fluminense@123";
            var url = "https://org20e4d7a3.crm2.dynamics.com";

            CrmServiceClient crmServiceClient = new CrmServiceClient(
                "AuthType=OAuth; " +
                $"Username={user};" +
                $"Password={password};" +
                $"Url={url};" +
                "AppId=ccdda0f7-8b2b-497e-8598-767622ac619c;"
                );

            Service = crmServiceClient.OrganizationWebProxyClient ;
        }

    }
}
