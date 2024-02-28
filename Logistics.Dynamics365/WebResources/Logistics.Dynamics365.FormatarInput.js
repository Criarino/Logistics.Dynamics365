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

    SetVisibleCPForCNPJ: function (context) {

        var formContext = context.getFormContext();
        var cpf = formContext.getAttribute("alfa_cpf").getValue();
        var cnpj = formContext.getAttribute("alfa_cnpj").getValue();

        if (cpf) {

            formContext.getControl("alfa_cpf").setVisible(true);

        }

        if (cnpj) {

            formContext.getControl("alfa_cnpj").setVisible(true);

        }

    },

    FormatarCNPJ: function (context) {
        var formContext = context.getFormContext();
        var cnpj = formContext.getAttribute("alfa_cnpj").getValue();

        function validarCNPJ(cnpjLimpo) {

            if(cnpjLimpo == '') return false;

            if (cnpjLimpo.length != 14)
                return false;

            
            if (cnpjLimpo == "00000000000000" ||
                cnpjLimpo == "11111111111111" ||
                cnpjLimpo == "22222222222222" ||
                cnpjLimpo == "33333333333333" ||
                cnpjLimpo == "44444444444444" ||
                cnpjLimpo == "55555555555555" ||
                cnpjLimpo == "66666666666666" ||
                cnpjLimpo == "77777777777777" ||
                cnpjLimpo == "88888888888888" ||
                cnpjLimpo == "99999999999999")
                return false;

           
            tamanho = cnpjLimpo.length - 2
            numeros = cnpjLimpo.substring(0, tamanho);
            digitos = cnpjLimpo.substring(tamanho);
            soma = 0;
            pos = tamanho - 7;
            for (i = tamanho; i >= 1; i--) {
                soma += numeros.charAt(tamanho - i) * pos--;
                if (pos < 2)
                    pos = 9;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(0))
                return false;

            tamanho = tamanho + 1;
            numeros = cnpjLimpo.substring(0, tamanho);
            soma = 0;
            pos = tamanho - 7;
            for (i = tamanho; i >= 1; i--) {
                soma += numeros.charAt(tamanho - i) * pos--;
                if (pos < 2)
                    pos = 9;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(1))
                return false;

            return true;
        }

        if (cnpj) {

            var cnpjLimpo = cnpj.replace(/[^\d]/g, '');

            if (validarCNPJ(cnpjLimpo)) {
                var cnpjFormatado = cnpjLimpo.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})$/, '$1.$2.$3/$4-$5');
                formContext.getAttribute("alfa_cnpj").setValue(cnpjFormatado);
            } else {
                Logistics.Utilidades.Alerta("Atenção", "CNPJ inválido");
                formContext.getAttribute("alfa_cnpj").setValue("");
            }
        }
    },

    FormatarCPF: function (context) {

        var formContext = context.getFormContext();
        var cpf = formContext.getAttribute("alfa_cpf").getValue();
                

        function validarCPF(cpflimpo) {

                        
            if (cpfLimpo.length !== 11 || /^(\d)\1{10}$/.test(cpf)) {
                return false; 
            }

            
            let sum = 0;
            for (let i = 0; i < 9; i++) {
                sum += parseInt(cpfLimpo.charAt(i)) * (10 - i);
            }
            let remainder = (sum * 10) % 11;
            let digit1 = (remainder === 10) ? 0 : remainder;

            
            sum = 0;
            for (let i = 0; i < 10; i++) {
                sum += parseInt(cpfLimpo.charAt(i)) * (11 - i);
            }
            remainder = (sum * 10) % 11;
            let digit2 = (remainder === 10) ? 0 : remainder;

            
            return (digit1 === parseInt(cpfLimpo.charAt(9)) && digit2 === parseInt(cpfLimpo.charAt(10)));
        }

        if (cpf) {

            var cpfLimpo = cpf.replace(/[^\d]/g, '');

            if (validarCPF(cpfLimpo)) {


                if (cpfLimpo.length === 11) {

                    var cpfFormatado = cpfLimpo.replace(/^(\d{3})(\d{3})(\d{3})(\d{2})$/, '$1.$2.$3-$4');


                    formContext.getAttribute("alfa_cpf").setValue(cpfFormatado);

                } else {

                    Logistics.Utilidades.Alerta("Atenção", "Insira um CPF válido")

                    formContext.getAttribute("alfa_cpf").setValue("");
                }

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


