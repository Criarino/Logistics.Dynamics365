using Logistics.Dynamics365.Plugins.Conexoes;
using Logistics.Dynamics365.Plugins.Repositorio;
using Microsoft.Crm.Sdk.Messages;
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

                string uomName = getUomName(uomId, Service);

                Trace.Trace("Cheguei aqui");

                EntityReference uomEntity = buildUomEntity(uomScheduleId, uomName, conn.Service);

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

        public string getUomName(Guid uomId, IOrganizationService service)
        {
            EntityCollection uomCollection = RepositorioUnidade.GetUomName(uomId, service);

            return (string)uomCollection.Entities.First()["name"];
        }

        public void OnDelete(Entity entity)
        {
            Trace.Trace("Conexxão iniciada");
            ConexaoDynamics conn = new ConexaoDynamics();
            Trace.Trace("Conexxão setada");
            DeleteOnAnotherEnv(entity, conn);
        }

        public void DeleteOnAnotherEnv(Entity entity, ConexaoDynamics conn)
        {
            try
            {
                conn.Service.Delete("product", entity.Id);
            }catch(Exception ex)
            {
                Trace.Trace(ex.Message);
                throw new InvalidPluginExecutionException("Não foi possivel criar o produto no ambiente.");
            }
        }

        public void OnUpdate(Entity entity)
        {
            Trace.Trace("Conexxão iniciada");
            ConexaoDynamics conn = new ConexaoDynamics();
            Trace.Trace("Conexxão setada");
            DeleteOnAnotherEnv(entity, conn);
        }

        public void UpdateOnAnotherEnv(Entity entity, ConexaoDynamics conn)
        {
            try
            {
                Entity newProduct = entity.Clone();

                if (entity["defaultuomid"] != null)
                {
                    Guid uomScheduleId = getUomScheduleIdByProduct(entity.Id,Service);
                    Guid uomId = entity.GetAttributeValue<EntityReference>("defaultuomid").Id;
                    string uomName = getUomName(uomId,Service);
                    EntityReference uomEntity = buildUomEntity(uomScheduleId, uomName, conn.Service);
                    newProduct["defaultuomid"] = uomEntity;
                }

                conn.Service.Update(newProduct);
            }
            catch (Exception ex)
            {
                Trace.Trace(ex.Message);
                throw new InvalidPluginExecutionException("Não foi possivel criar o produto no ambiente.");
            }
        }

        public Guid getUomScheduleIdByProduct(Guid productId,IOrganizationService service)
        {
            string[] queryCollumns = { "defaultuomscheduleid" };
            EntityCollection productCollection = RepositorioProduto.getProduct(productId, queryCollumns, service);

            return productCollection.Entities.First().GetAttributeValue<EntityReference>("defaultuomscheduleid").Id;
        } 

        private EntityReference buildUomEntity(Guid uomScheduleId, string uomName, IOrganizationService service)
        {
            EntityCollection uomCollection = RepositorioUnidade.GetUom(uomScheduleId, uomName, service);
            Guid newUomID = uomCollection.Entities.First().GetAttributeValue<Guid>("uomid");
            return new EntityReference("uom", newUomID);
        }
    }
}
