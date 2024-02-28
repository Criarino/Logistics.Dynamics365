using Logistics.Dynamics365.Plugins.Conexoes;
using Logistics.Dynamics365.Plugins.Repositorio;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;

namespace Logistics.Dynamics365.Plugins.Gerenciadores
{
    public class GerenciadorProduto : IGerenciadorPlugin
    {
        public IOrganizationService Service { get; set; }
        public ITracingService Trace { get; set; }


        public GerenciadorProduto(IOrganizationService service, ITracingService trace)
        {
            Service = service;
            Trace = trace;
        }

        public void OnCreate(Entity product)
        {
            Trace.Trace("Conexxão iniciada");
            ConexaoDynamics conn = new ConexaoDynamics();
            Trace.Trace("Conexxão setada");
            CreateOnAnotherEnv(product, conn);
            Trace.Trace("Integração finalizada");
        }

        public void CreateOnAnotherEnv(Entity product, ConexaoDynamics conn)
        {
            try {

                Entity newProduct = product.Clone();
                Guid uomScheduleId = product.GetAttributeValue<EntityReference>("defaultuomscheduleid").Id;
                Guid uomId = product.GetAttributeValue<EntityReference>("defaultuomid").Id;

                string uomName = getUomName(uomId);

                Trace.Trace("Cheguei aqui");


                EntityCollection uomCollection = RepositorioUnidade.GetUom(uomScheduleId, uomName, conn.Service);

                Trace.Trace("Cheguei aqui 2");

                Guid newUomID = uomCollection.Entities.First().GetAttributeValue<Guid>("uomid");
                EntityReference uomEntity = new EntityReference("uom", newUomID);
                //Entity uomEntity = new Entity(uomName, newUomID);

                Trace.Trace("Cheguei  3");


                newProduct["defaultuomid"] = uomEntity;

                Trace.Trace("Cheguei 4");


                var id = conn.Service.Create(newProduct);
            }catch(Exception ex)
            {
                Trace.Trace(ex.Message);
                throw new InvalidPluginExecutionException("Não foi possivel criar o produto no ambiente.");
            }

        }

        public string getUomName(Guid uomId)
        {
            EntityCollection uomCollection = RepositorioUnidade.GetUomName(uomId, Service);

            return (string)uomCollection.Entities.First()["name"];
        }

        public void OnDelete(Entity entity)
        {
            throw new NotImplementedException();
        }

        public void OnUpdate(Entity entity)
        {
            throw new NotImplementedException();
        }
    }
}
