using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins.Repositorio
{
    public static class RepositorioUnidade
    {
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
            EntityCollection teste = service.RetrieveMultiple(query);

            return service.RetrieveMultiple(query);
        }

    }
}
