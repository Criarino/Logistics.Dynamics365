if (typeof (Logistics) === "undefined") Logistics = {};
if (typeof (Logistics.Cep) === "undefined") Logistics.Cep = {};

Logistics.Cep = {
    OnCepChange: function (context) {
        const formContext = context.getFormContext();
        const cepValue = formContext.getAttribute("address1_postalcode").getValue();

        Xrm.Utility.showProgressIndicator("Buscando CEP...");

        if (!this.cepIsValid(cepValue)) {
            Xrm.Utility.closeProgressIndicator();
            Logistics.Utilidades.Alerta("Atenção", "Insira um CEP válido")
            this.clearFields(formContext)
        }

        if (this.cepIsValid(cepValue)) {

            this.fetchCep(cepValue)
                .then(res => {
                    if (res.erro) {
                        Xrm.Utility.closeProgressIndicator();
                        Logistics.Utilidades.Alerta("Atenção", "Insira um CEP válido")
                        this.clearFields(formContext)
                    } else {
                        const endereço = `${res.logradouro}, ${res.bairro}${res.complemento ? ", " + res.complemento : ""}`
                        this.setFormAtributte(formContext, "address1_postalcode", res.cep)
                        this.setFormAtributte(formContext, "address1_line1", endereço)
                        this.setFormAtributte(formContext, "address1_city", res.localidade)
                        this.setFormAtributte(formContext, "address1_stateorprovince", this.getStateName(res.uf))
                        this.setFormAtributte(formContext, "alfa_endereco1_codigoibge", res.ibge)
                        this.setFormAtributte(formContext, "alfa_endereco1_ddd", res.ddd)
                    }

                })
        }
        Xrm.Utility.closeProgressIndicator();
    },

    fetchCep: function (cep) {
        return fetch(`https://viacep.com.br/ws/${cep}/json/`)
            .then(res => res.json())
    },

    cepIsValid: function (cep) {
        let validCep = cep.replace(/\s+|-/g, "");


        if (validCep.length !== 8 || !validCep) {
            return false
        }

        return validCep.split("").every((char) => !isNaN(char));
    },

    setFormAtributte: function (formContext, name, value) {

        if (formContext.getAttribute(name)) {
            formContext.getAttribute(name).setValue(value)
        }
    },

    getStateName: function (uf) {
        return `${this.Estados[uf]}, ${uf}`
    },

    clearFields: function (formContext) {
        this.setFormAtributte(formContext, "address1_postalcode", null)
        this.setFormAtributte(formContext, "address1_line1", null)
        this.setFormAtributte(formContext, "address1_city", null)
        this.setFormAtributte(formContext, "address1_stateorprovince", null)
        this.setFormAtributte(formContext, "alfa_endereco1_codigoibge", null)
        this.setFormAtributte(formContext, "alfa_endereco1_ddd", null)
    },

    Estados: {
        'AC': 'Acre',
        'AL': 'Alagoas',
        'AP': 'Amapá',
        'AM': 'Amazonas',
        'BA': 'Bahia',
        'CE': 'Ceará',
        'DF': 'Distrito Federal',
        'ES': 'Espirito Santo',
        'GO': 'Goiás',
        'MA': 'Maranhão',
        'MS': 'Mato Grosso do Sul',
        'MT': 'Mato Grosso',
        'MG': 'Minas Gerais',
        'PA': 'Pará',
        'PB': 'Paraíba',
        'PR': 'Paraná',
        'PE': 'Pernambuco',
        'PI': 'Piauí',
        'RJ': 'Rio de Janeiro',
        'RN': 'Rio Grande do Norte',
        'RS': 'Rio Grande do Sul',
        'RO': 'Rondônia',
        'RR': 'Roraima',
        'SC': 'Santa Catarina',
        'SP': 'São Paulo',
        'SE': 'Sergipe',
        'TO': 'Tocantins'
    }

}