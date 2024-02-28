using Logistics.Dynamics365.Plugins.Gerenciadores;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins
{
    public class GrupoDeUnidadesPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            Entity grupoDeUnidade = (Entity)context.InputParameters["Target"];
            GerenciadorGrupoDeUnidade gerenciadorGrupoDeUnidade = new GerenciadorGrupoDeUnidade(service, trace);

            if (context.MessageName.Equals("Create"))
            {
                try
                {
                    trace.Trace("Integrando Grupo de Unidade....");
                    gerenciadorGrupoDeUnidade.OnCreate(grupoDeUnidade);
                }
                catch (Exception ex)
                {
                    trace.Trace(ex.Message);
                }


            }



        }
    }
}
