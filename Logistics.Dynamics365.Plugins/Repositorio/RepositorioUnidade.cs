using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins.Repositorio
{
    public static class RepositorioUnidade
    {
        ///<summary>
        ///Esse metodo retorna a Unidae com os atributos uomid,nome,uomscheduleid  atraves do nome e uomscheduleid inseridos por paremetro.;
        ///</summary>
        public static EntityCollection GetUom(Guid uomScheduleId, string uomName , IOrganizationService service)
        {
            QueryExpression query = new QueryExpression("uom");
            FilterExpression filter = new FilterExpression();
            filter.FilterOperator = LogicalOperator.And;
            filter.AddCondition("uomscheduleid", ConditionOperator.Equal, uomScheduleId.ToString());
            filter.AddCondition("name", ConditionOperator.Equal, uomName);

            query.ColumnSet.AddColumns("uomid", "name", "uomscheduleid");
            query.Criteria.AddFilter(filter);

            return service.RetrieveMultiple(query);
        }

        public static EntityCollection GetUomName(Guid uomId, IOrganizationService service)
        {
            QueryExpression query = new QueryExpression("uom");
            FilterExpression filter = new FilterExpression();
            filter.FilterOperator = LogicalOperator.And;
            filter.AddCondition("uomid", ConditionOperator.Equal, uomId.ToString());

            query.ColumnSet.AddColumns("uomid", "name", "uomscheduleid");
            query.Criteria.AddFilter(filter);

            return service.RetrieveMultiple(query);
        }

        ///<summary>
        ///Esse metodo retorna as Unidades com os atributos passados por paremetro atraves do filtro correspondente.;
        ///</summary>
        public static EntityCollection GetUom(string[] collumns,FilterExpression filter, IOrganizationService service)
        {
            QueryExpression query = new QueryExpression("uom");
            query.ColumnSet.AddColumns(collumns);
            query.Criteria.AddFilter(filter);

            return service.RetrieveMultiple(query);
        }
        ///<summary>
        ///Esse metodo retorna a Unidade com os atributos passados por paremetro atraves do 'uomid' correspondente.;
        ///</summary>
        public static EntityCollection GetUom(Guid uomId,string[] collumns, IOrganizationService service)
        {
            QueryExpression query = new QueryExpression("uom");
            query.ColumnSet.AddColumns(collumns);
            query.Criteria.AddCondition("uomid", ConditionOperator.Equal, uomId.ToString()) ;

            return service.RetrieveMultiple(query);
        }



    }
}
