using Logistics.Dynamics365.Plugins.Repositorios;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Dynamics365.Plugins.Gerenciadores
{
    public class Gerenciador
    {
        private IOrganizationService Service { get; set; }

        public Gerenciador(IOrganizationService service)
        {
            Service = service;


        }

        public void ValidarDuplicidadeCNPJ(Entity conta)
        {
            if (conta.Contains("alfa_cnpj"))
            {
                string cnpj = conta["alfa_cnpj"].ToString();

                var contas = Repositorio.BuscarContaPorCNPJ(cnpj, Service);

                if (contas.Entities.Count() > 0)
                {
                    throw new InvalidPluginExecutionException("CNPJ já existente");
                }


            }
        }

        public void ValidarDuplicidadeCPF(Entity conta)
        {

            
            if (conta.Contains("alfa_cpf"))
            {
                string cpf = conta["alfa_cpf"].ToString();

                var contas = Repositorio.BuscarContaPorCPF(cpf, Service);

               
                if (contas.Entities.Count() > 0)
                {
                    throw new InvalidPluginExecutionException("CPF já existente");
                }


            }
        }


        public void ValidarDuplicidadeCPFContato(Entity contato)
        {


            if (contato.Contains("alfa_cpf"))
            {
                string cpf = contato["alfa_cpf"].ToString();

                var contatos = Repositorio.BuscarContatoPorCPF(cpf, Service);


                if (contatos.Entities.Count() > 0)
                {
                    throw new InvalidPluginExecutionException("CPF já existente");
                }


            }
        }

    }
}
