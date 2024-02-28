using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace Logistics.Dynamics365.Plugins
{
    public class ContaPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            var Letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var Numeros = "0123456789";
            var stringCharsLetras = new char[4];
            var stringCharsNumeros = new char[4];
            var random = new Random();
            var repeats = 0;

            Repeat: //label para repetir a geração de código caso já exista

            for (int i = 0; i < stringCharsNumeros.Length; i++) //criando sequencia de numeros aleatórios
            {
                stringCharsNumeros[i] = Numeros[random.Next(Numeros.Length)];
            }

            for (int i = 0; i < stringCharsLetras.Length; i++) //criando sequencia de letras aleatórias
            {
                stringCharsLetras[i] = Letras[random.Next(Letras.Length)];
            }

            var fim = new String(stringCharsLetras);
            var meio = new String(stringCharsNumeros);

            var finalString = "OPP-" + meio + "-" + fim; //concatenando as sequencias

            Entity oportunidade = (Entity)context.InputParameters["Target"];

            if (oportunidade.Contains("alfa_codigounico"))//conferir código duplicado
            {
                QueryExpression query = new QueryExpression("opportunity");
                query.ColumnSet.AddColumns("alfa_codigounico");
                query.Criteria.AddCondition("alfa_codigounico", ConditionOperator.Equal, finalString);
                EntityCollection OpportiunityCollection = service.RetrieveMultiple(query);
                
                if (OpportiunityCollection.Entities.Count() > 0)
                {
                    repeats++;
                    if (repeats <= 10) 
                    {
                        goto Repeat;
                    }
                    else//caso entre em loop (repetiu 10x), dispara exceção
                    {
                        throw new InvalidPluginExecutionException("Erro ao gerar código único");
                    }
                }
            }
            oportunidade["alfa_codigounico"] = finalString; //atribuindo a sequencia gerada ao campo alfa_codigounico
        }
    }
}
