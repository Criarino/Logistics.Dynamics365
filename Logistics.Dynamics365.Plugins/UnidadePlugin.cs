using Logistics.Dynamics365.Plugins.Gerenciadores;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins
{
    public class UnidadePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            GerenciadorUnidade gerenciadorUnidade = new GerenciadorUnidade(service, trace);

            if (context.MessageName.Equals("Create") || context.MessageName.Equals("Update"))
            {
                Entity unidade = (Entity)context.InputParameters["Target"];
                if (context.MessageName.Equals("Create"))
                {
                    try
                    {
                        trace.Trace("Integrando Unidade....");
                        gerenciadorUnidade.OnCreate(unidade);
                    }
                    catch (Exception ex)
                    {
                        trace.Trace(ex.Message);
                    }
                }
                else if (context.MessageName.Equals("Update"))
                {
                    try
                    {
                        Entity preImg = context.PreEntityImages["preuom"];
                        trace.Trace("(Update)Integrando Unidade....");
                        unidade["prename"] = preImg["name"];
                        unidade["preuomscheduleid"] = preImg["uomscheduleid"];
                        gerenciadorUnidade.OnUpdate(unidade);
                    }
                    catch (Exception ex)
                    {
                        trace.Trace(ex.Message);
                    }


                }
            }else if (context.MessageName.Equals("Delete"))
            {
                Guid unidadeId = context.PrimaryEntityId;
                try
                {
                    trace.Trace("(Update)Integrando Unidade....");
                    gerenciadorUnidade.OnDelete(unidadeId);
                }
                catch (Exception ex)
                {
                    trace.Trace(ex.Message);
                }
            }




        }
    }
}
