using Logistics.Dynamics365.Plugins.Gerenciadores;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins
{
    public class OportunidadePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            GerenciadorOportunidade gerenciadorOportunidade = new GerenciadorOportunidade(service, trace);
            Entity opportunity = (Entity)context.InputParameters["Target"];

            if (context.MessageName.Equals("Create"))
            {
                try
                {
                    trace.Trace("Integrando Oportunidade....");
                    gerenciadorOportunidade.OnCreate(opportunity);
                }
                catch (Exception ex)
                {
                    trace.Trace(ex.Message);
                }
            }
            /*else if (context.MessageName.Equals("Update"))
            {
                try
                {
                    trace.Trace("(UPDATE) Integrando Produto....");
                    gerenciadorOportunidade.OnUpdate(opportunity);

                }
                catch (Exception ex)
                {
                    trace.Trace(ex.Message);
                }

            }
            else if (context.MessageName.Equals("Delete"))
            {
                try
                {

                    trace.Trace("(UPDATE) Integrando Produto....");
                    gerenciadorOportunidade.OnDelete(opportunity);

                }
                catch (Exception ex)
                {
                    trace.Trace(ex.Message);
                }
            }*/
        }
    }
}