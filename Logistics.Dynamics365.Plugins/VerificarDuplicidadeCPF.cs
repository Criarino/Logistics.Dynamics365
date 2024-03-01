using Logistics.Dynamics365.Plugins.Gerenciadores;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins
{
    public class DuplicidadeCPFPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            ITracingService trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));


            #region VERIFICAÇÃO DE DUPLICIDADE DO CPF

            if (context.MessageName == "Create" || context.MessageName == "Update")
            {
                Entity conta = (Entity)context.InputParameters["Target"];
                Gerenciador gerenciadorConta = new Gerenciador(service);
                gerenciadorConta.ValidarDuplicidadeCPF(conta);
                
            }

            #endregion
        }
    }
}
