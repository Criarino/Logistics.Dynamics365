using Logistics.Dynamics365.Plugins.Conexoes;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;

namespace Logistics.Dynamics365.Plugins.Gerenciadores
{
    public class GerenciadorProduto
    {
        private IOrganizationService Service { get; set; }
        private ITracingService Trace { get; set; }

        public GerenciadorProduto(IOrganizationService service, ITracingService trace)
        {
            Service = service;
            Trace = trace;
        }

        public void OnCreateProudct(Entity product)
        {
            Trace.Trace("Conexxão iniciada");
            ConexaoDynamics conn = new ConexaoDynamics();
            Entity conta = new Entity("account");
            conta["name"] = "Teste";
            var idConta = conn.Service.Create(conta);
            Trace.Trace("Conexxão setada");
            CreateOnAnotherEnv(product, conn);
            Trace.Trace("Integração finalizada");
        }

        public void CreateOnAnotherEnv(Entity product, ConexaoDynamics conn)
        {
            try { 
                var id = conn.Service.Create(product);
            }catch(Exception ex)
            {
                Trace.Trace(ex.Message);
                throw new InvalidPluginExecutionException("Não foi possivel criar o produto no ambiente.");
            }

        }
    }
}
