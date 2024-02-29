using Logistics.Dynamics365.Plugins.Conexoes;
using Logistics.Dynamics365.Plugins.Repositorio;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
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
            ConexaoDynamics conn = new ConexaoDynamics();
            CreateOnAnotherEnv(product, conn);
        }

        public void CreateOnAnotherEnv(Entity product, ConexaoDynamics conn)
        {
            try {

                Entity newProduct = product.Clone();
                Guid uomScheduleId = product.GetAttributeValue<EntityReference>("defaultuomscheduleid").Id;
                Guid uomId = product.GetAttributeValue<EntityReference>("defaultuomid").Id;
                string uomName = getUomName(uomId, Service);
                EntityReference uomEntity = buildUomEntity(uomScheduleId, uomName, conn.Service);

                newProduct["defaultuomid"] = uomEntity;

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

        public void OnDelete(Guid produtoId)
        {
            ConexaoDynamics conn = new ConexaoDynamics();
            conn.Service.Delete("product", produtoId);

        }

        public void OnUpdate(Entity entity)
        {
            ConexaoDynamics conn = new ConexaoDynamics();
            UpdateOnAnotherEnv(entity, conn);
        }

        public void UpdateOnAnotherEnv(Entity entity, ConexaoDynamics conn)
        {
            try
            {
                Entity newProduct = entity.Clone();

                if (entity.Contains("defaultuomid"))
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

        private static EntityReference buildUomEntity(Guid uomScheduleId, string uomName, IOrganizationService service)
        {
            EntityCollection uomCollection = RepositorioUnidade.GetUom(uomScheduleId, uomName, service);
            Guid newUomID = uomCollection.Entities.First().GetAttributeValue<Guid>("uomid");

            return new EntityReference("uom", newUomID);
        }
    }
}
