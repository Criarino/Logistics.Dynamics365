using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins.Repositorios
{
    public static class Repositorio
    {

        public static EntityCollection BuscarContaPorCNPJ(string cnpj, IOrganizationService service)
        {
            QueryExpression query = new QueryExpression("account");
            query.ColumnSet.AddColumns("accountid", "name");
            query.Criteria.AddCondition("alfa_cnpj", ConditionOperator.Equal, cnpj);
            EntityCollection contasCollection = service.RetrieveMultiple(query);
            return contasCollection;
        }

        public static EntityCollection BuscarContaPorCPF(string cpf, IOrganizationService service)
        {
            QueryExpression query = new QueryExpression("account");
            query.ColumnSet.AddColumns("accountid", "name");
            query.Criteria.AddCondition("alfa_cpf", ConditionOperator.Equal, cpf);
            EntityCollection contasCollection = service.RetrieveMultiple(query);
            return contasCollection;
        }

        public static EntityCollection BuscarContatoPorCPF(string cpf, IOrganizationService service)
        {
            QueryExpression query = new QueryExpression("contact");
            query.ColumnSet.AddColumns("contactid", "fullname");
            query.Criteria.AddCondition("alfa_cpf", ConditionOperator.Equal, cpf);
            EntityCollection contasCollection = service.RetrieveMultiple(query);
            return contasCollection;
        }

    }
}
