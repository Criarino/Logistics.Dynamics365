using Logistics.Dynamics365.Plugins.Conexoes;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins.Gerenciadores
{
    public class GerenciadorOportunidade : IGerenciadorPlugin
    {
        public IOrganizationService Service { get; set; }
        public ITracingService Trace { get; set; }

        public GerenciadorOportunidade(IOrganizationService service, ITracingService trace)
        {
            Service = service;
            Trace = trace;
        }

        public void OnCreate(Entity opportunity)
        {
            Trace.Trace("Conexxão iniciada");
            ConexaoDynamics conn = new ConexaoDynamics();
            Trace.Trace("Conexxão setada");
            CreateOnAnotherEnv(opportunity, conn);
            Trace.Trace("Integração finalizada");
        }
        public void OnDelete(Entity entity)
        {
            throw new NotImplementedException();
        }
        public void OnUpdate(Entity entity)
        {
            throw new NotImplementedException();
        }

        public void CreateOnAnotherEnv(Entity opportunity, ConexaoDynamics conn)
        {
            try
            {
                Entity newOpportunity = opportunity.Clone();
                Trace.Trace("Teste !!");
                var id = conn.Service.Create(newOpportunity);
            }
            catch (Exception ex)
            {
                Trace.Trace(ex.Message);
                throw new InvalidPluginExecutionException("Não foi possivel criar a oportunidade no ambiente.");
            }
        }
    }
}
