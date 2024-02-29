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
            conn.Service.Delete("uom", entityId);

        }

        public void OnUpdate(Entity entity)
        {
            Trace.Trace("Conexxão iniciada");
            ConexaoDynamics conn = new ConexaoDynamics();
            Trace.Trace("Conexxão setada");
            UpdateOnAnotherEnv(entity, conn);
            Trace.Trace("Integração finalizada");
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
                Trace.Trace("cheguei 1");
                if (!hasMatch(entity.Id,conn.Service)){
                    Trace.Trace("cheguei 2");

                    string[] collumns = { "name", "uomscheduleid" };
                    Trace.Trace("cheguei 3");

                    EntityCollection uomCollection = RepositorioUnidade.GetUom(entity.Id, collumns, Service);
                    Trace.Trace("cheguei 4");

                    Entity originalUom = uomCollection.Entities.First();
                    //Entity originalUom = getUomAtributtes(entity, collumns, Service);

                    string uomName = (string)entity["prename"];
                    Trace.Trace("cheguei 5");

                    Guid uomScheduleId = entity.GetAttributeValue<EntityReference>("preuomscheduleid").Id;
                    Trace.Trace("cheguei 6");


                    EntityReference uomEntity = buildUomEntity(uomScheduleId, uomName, conn.Service);
                    Trace.Trace("cheguei 7");

                    newEntity.Id = uomEntity.Id;
                    newEntity["uomid"] = uomEntity.Id;
                    Trace.Trace("cheguei 8");

                }


                if (entity.Contains("baseuom"))
                {
                    if(!hasMatch(entity.GetAttributeValue<EntityReference>("baseuom").Id, conn.Service))
                    {
                        string[] collumnsBaseUom = { "name", "uomscheduleid" };
                        Trace.Trace("cheguei 9");

                        Guid baseUomId = entity.GetAttributeValue<EntityReference>("baseuom").Id;
                        Trace.Trace("cheguei 10");

                        EntityCollection baseUomCollection = RepositorioUnidade.GetUom(baseUomId, collumnsBaseUom, Service);
                        Trace.Trace("cheguei 11");


                        Entity originalBaseUom = baseUomCollection.Entities.First();
                        Trace.Trace("cheguei 12");

                        string baseUomName = (string)originalBaseUom["name"];
                        Trace.Trace("cheguei 13");

                        Guid uomScheduleId = originalBaseUom.GetAttributeValue<EntityReference>("uomscheduleid").Id;
                        Trace.Trace("cheguei 14");



                        EntityReference baseUomEntity = buildUomEntity(uomScheduleId, baseUomName, conn.Service);
                        Trace.Trace("cheguei 15");

                        newEntity["baseuom"] = baseUomEntity;
                        Trace.Trace("cheguei 16");

                    }

                }
                Trace.Trace("cheguei 17");

                newEntity.Attributes.Remove("prename");
                newEntity.Attributes.Remove("preuomscheduleid");
                conn.Service.Update(newEntity);
                Trace.Trace("cheguei 18");

            }
            catch (Exception ex)
            {
                Trace.Trace(ex.StackTrace);
                Trace.Trace(ex.Message);
                Trace.Trace(ex.Source);
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
            Trace.Trace("Hit");
            string[] collumns = { "uomid" };
            Trace.Trace("Hit2");
            EntityCollection uomCollection = RepositorioUnidade.GetUom(uomid,collumns,service);
            Trace.Trace("Hit3");
            return uomCollection.Entities.Count() > 0;
        }

    }
}
