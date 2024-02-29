using Logistics.Dynamics365.Plugins.Conexoes;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins.Gerenciadores
{
    public class GerenciadorGrupoDeUnidade : IGerenciadorPlugin
    {
        public IOrganizationService Service { get; set; }
        public ITracingService Trace { get; set; }

        public GerenciadorGrupoDeUnidade(IOrganizationService service, ITracingService trace)
        {
            Service = service;
            Trace = trace;
        }

        public void OnCreate(Entity entity)
        {
            Trace.Trace("Conexxão iniciada");
            ConexaoDynamics conn = new ConexaoDynamics();
            Trace.Trace("Conexxão setada");
            CreateOnAnotherEnv(entity, conn);
            Trace.Trace("Integração finalizada");
        }

        public void OnDelete(Guid entityId)
        {
            Trace.Trace("Conexxão iniciada");
            ConexaoDynamics conn = new ConexaoDynamics();
            Trace.Trace("Conexxão setada");
            conn.Service.Delete("uomschedule", entityId);
        }

        public void OnUpdate(Entity entity)
        {
            throw new NotImplementedException();
        }

        public void CreateOnAnotherEnv(Entity entity, ConexaoDynamics conn)
        {
            try
            {
                var id = conn.Service.Create(entity);
            }
            catch (Exception ex)
            {
                Trace.Trace(ex.Message);
                throw new InvalidPluginExecutionException("Não foi possivel criar o produto no ambiente.");
            }

        }
    }
}
