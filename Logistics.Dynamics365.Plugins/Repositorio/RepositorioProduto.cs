using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins.Repositorio
{
    public static class RepositorioProduto
    {
        public static EntityCollection getProduct(Guid productId,string[] collumns, IOrganizationService service)
        {
            QueryExpression query = new QueryExpression("product");
            query.ColumnSet.AddColumns(collumns);
            query.Criteria.AddCondition("productid", ConditionOperator.Equal, productId.ToString());

            return service.RetrieveMultiple(query);
        }
    }
}
