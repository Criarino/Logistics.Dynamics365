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
            ConexaoDynamics conn = new ConexaoDynamics();
            conn.Service.Create(entity);
        }

        public void OnDelete(Guid entityId)
        {
            ConexaoDynamics conn = new ConexaoDynamics();
            conn.Service.Delete("uomschedule", entityId);
        }

        public void OnUpdate(Entity entity)
        {
            ConexaoDynamics conn = new ConexaoDynamics();
            conn.Service.Update(entity);
        }

    }
}
