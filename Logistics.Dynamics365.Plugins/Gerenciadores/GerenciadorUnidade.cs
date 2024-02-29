using Logistics.Dynamics365.Plugins.Conexoes;
using Logistics.Dynamics365.Plugins.Repositorio;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;

namespace Logistics.Dynamics365.Plugins.Gerenciadores
{
    public class GerenciadorUnidade : IGerenciadorPlugin
    {
        public IOrganizationService Service { get; set; }
        public ITracingService Trace { get; set; }

        public GerenciadorUnidade(IOrganizationService service, ITracingService trace)
        {
            Service = service;
            Trace = trace;
        }

        public void OnCreate(Entity entity)
        {
            ConexaoDynamics conn = new ConexaoDynamics();
            CreateOnAnotherEnv(entity, conn);
        }

        public void OnDelete(Guid entityId)
        {
            ConexaoDynamics conn = new ConexaoDynamics();
            conn.Service.Delete("uom", entityId);
        }

        public void OnUpdate(Entity entity)
        {
            ConexaoDynamics conn = new ConexaoDynamics();
            UpdateOnAnotherEnv(entity, conn);
        }

        public void CreateOnAnotherEnv(Entity entity, ConexaoDynamics conn)
        {
            try
            {
                Entity newEntity = entity.Clone();

                Guid uomScheduleId = entity.GetAttributeValue<EntityReference>("uomscheduleid").Id;

                if (entity.Contains("baseuom"))
                {
                    Guid baseUomId = entity.GetAttributeValue<EntityReference>("baseuom").Id;
                    if(!hasMatch(baseUomId, conn.Service)){
                        string[] collumns = { "name" };
                        EntityCollection baseUomCollection = RepositorioUnidade.GetUom(baseUomId, collumns, Service);

                        Entity originalBaseUom = baseUomCollection.Entities.First();
                        string baseUomName = (string)originalBaseUom["name"];

                        EntityReference baseUomEntity = buildUomEntity(uomScheduleId, baseUomName, conn.Service);
                        newEntity["baseuom"] = baseUomEntity;
                    }
                }

                var id = conn.Service.Create(newEntity);
            }
            catch (Exception ex)
            {
                Trace.Trace(ex.Message);
                throw new InvalidPluginExecutionException("Não foi possivel criar o produto no ambiente.");
            }

        }

        public void UpdateOnAnotherEnv(Entity entity, ConexaoDynamics conn)
        {
            try
            {
                Entity newEntity = entity.Clone();

                if (!hasMatch(entity.Id,conn.Service)){
                    string[] collumns = { "name", "uomscheduleid" };
                    EntityCollection uomCollection = RepositorioUnidade.GetUom(entity.Id, collumns, Service);

                    string uomName = (string)entity["prename"];
                    Guid uomScheduleId = entity.GetAttributeValue<EntityReference>("preuomscheduleid").Id;
                    EntityReference uomEntity = buildUomEntity(uomScheduleId, uomName, conn.Service);

                    newEntity.Id = uomEntity.Id;
                    newEntity["uomid"] = uomEntity.Id;
                }


                if (entity.Contains("baseuom"))
                {
                    if(!hasMatch(entity.GetAttributeValue<EntityReference>("baseuom").Id, conn.Service))
                    {
                        string[] collumnsBaseUom = { "name", "uomscheduleid" };
                        Guid baseUomId = entity.GetAttributeValue<EntityReference>("baseuom").Id;

                        EntityCollection baseUomCollection = RepositorioUnidade.GetUom(baseUomId, collumnsBaseUom, Service);
                        Entity originalBaseUom = baseUomCollection.Entities.First();
                        string baseUomName = (string)originalBaseUom["name"];
                        Guid uomScheduleId = originalBaseUom.GetAttributeValue<EntityReference>("uomscheduleid").Id;

                        EntityReference baseUomEntity = buildUomEntity(uomScheduleId, baseUomName, conn.Service);
                        newEntity["baseuom"] = baseUomEntity;

                    }

                }

                newEntity.Attributes.Remove("prename");
                newEntity.Attributes.Remove("preuomscheduleid");
                conn.Service.Update(newEntity);
            }
            catch (Exception ex)
            {
                Trace.Trace(ex.Message);
                throw new InvalidPluginExecutionException("Não foi possivel criar o produto no ambiente.");
            }

        }

        private static EntityReference buildUomEntity(Guid uomScheduleId, string uomName, IOrganizationService service)
        {
            EntityCollection uomCollection = RepositorioUnidade.GetUom(uomScheduleId, uomName, service);
            Guid newUomID = uomCollection.Entities.First().GetAttributeValue<Guid>("uomid");

            return new EntityReference("uom", newUomID);
        }

        private bool hasMatch(Guid uomid,IOrganizationService service)
        {
            string[] collumns = { "uomid" };
            EntityCollection uomCollection = RepositorioUnidade.GetUom(uomid,collumns,service);

            return uomCollection.Entities.Count() > 0;
        }

    }
}
