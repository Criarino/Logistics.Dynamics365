if (typeof (Logistics) === "undefined") Logistics = {};
if (typeof (Logistics.FormatarInputs) === "undefined") Logistics.FormatarInputs = {};

Logistics.FormatarInputs = {

    Enumerador: {

        tipoCliente: {

            pessoaFisica: 416860000,
            pessoaJuridica: 416860001,
            internacional: 416860002

        }

    },

    DefinirTipoCliente: function (context) {

        var formContext = context.getFormContext();
        var tipoCliente = formContext.getAttribute("alfa_tipocliente").getValue();

        if (tipoCliente === Logistics.FormatarInputs.Enumerador.tipoCliente.pessoaFisica) {  /*pessoa fisica*/

            formContext.getControl("alfa_cpf").setVisible(true);
            formContext.getControl("alfa_cnpj").setVisible(false);

        } else if (tipoCliente === Logistics.FormatarInputs.Enumerador.tipoCliente.pessoaJuridica) /*pessoa juridica*/ {

            formContext.getControl("alfa_cnpj").setVisible(true)
            formContext.getControl("alfa_cpf").setVisible(false)

        } else if (tipoCliente === Logistics.FormatarInputs.Enumerador.tipoCliente.internacional) {

            formContext.getControl("alfa_cnpj").setVisible(false)
            formContext.getControl("alfa_cpf").setVisible(false)

        }

    },

    FormatarCNPJ: function (context) {

        var formContext = context.getFormContext();
        var cnpj = formContext.getAttribute("alfa_cnpj").getValue();


        if (cnpj) {

            var cnpjLimpo = cnpj.replaceAll(".", "").replace("/", "").replace("-", "");

            if (cnpjLimpo.length == 14) {

                
                var cnpjFormatado = cnpjLimpo.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})$/, '$1.$2.$3/$4-$5');

                formContext.getAttribute("alfa_cnpj").setValue(cnpjFormatado);

            } else {

                Logistics.Utilidades.Alerta("Atenção", "Insira um CNPJ válido")
                formContext.getAttribute("alfa_cnpj").setValue("");
            }
        }
        
    },

    FormatarCPF: function (context) {

        var formContext = context.getFormContext();
        var cpf = formContext.getAttribute("alfa_cpf").getValue();

        if (cpf) {

            var cpfLimpo = cpf.replace(/[^\d]/g, '');

            if (cpfLimpo.length === 11) {
               
                var cpfFormatado = cpfLimpo.replace(/^(\d{3})(\d{3})(\d{3})(\d{2})$/, '$1.$2.$3-$4');

               
                formContext.getAttribute("alfa_cpf").setValue(cpfFormatado);
                
            } else {

                Logistics.Utilidades.Alerta("Atenção", "Insira um CPF válido")
                
                formContext.getAttribute("alfa_cpf").setValue("");
            }

        }

    },

    FormatarNomeConta: function (context) {


        var formContext = context.getFormContext();
        var nomeConta = formContext.getAttribute("name").getValue();

        if (nomeConta) {

            var nomeFormatado = nomeConta.toLowerCase().replace(/\b\w/g, function (firstLetter) {

                return firstLetter.toUpperCase();

            });

            
            formContext.getAttribute("name").setValue(nomeFormatado);

        }

    },

}


