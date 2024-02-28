if (typeof (Logistics) === "undefined") Logistics = {};
if (typeof (Logistics.Cep) === "undefined") Logistics.Cep = {};

Logistics.Cep = {
    OnCepChange: function (context) {
        console.log("hit1")
        const formContext = context.getFormContext();
        const cepValue = formContext.getAttribute("address1_postalcode").getValue();

        if (!this.cepIsValid(cep)) Logistics.Utilidades.Alerta("Atenção", "Insira um CEP válido")

        if (this.cepIsValid(cep)) {
            this.fetchCep(cep)
                .then(res => {
                    this.setFormAtributte(formContext, "address1_line1", res.logradouro)
                    //this.setFormAtributte(formContext, "address1_line1", res.complemento)
                    //this.setFormAtributte(formContext, "address1_line1", res.bairro)
                    //this.setFormAtributte(formContext, "address1_line1", res.localidade)
                    //this.setFormAtributte(formContext, "address1_line1", res.uf)
                    //this.setFormAtributte(formContext, "address1_line1", res.ibge)
                    //this.setFormAtributte(formContext, "address1_line1", res.ddd)
                })
        }
    },

    FetchCep: function (cep) {
        return fetch(`viacep.com.br/ws/${cep}/json/`)
            .then(res => res.json())
    },

    CepIsValid: function (cep) {
        let validCep = cep.replace(/\s+|-/g, "");

        if (validCep.length !== 8) {
            return false
        }

        return validCep.split("").every((char) => !isNaN(char));
    },

    SetFormAtributte: function (formContext, name, value) {
        if (formContext.getAttribute(name)) {
            formContext.getAttribute(name).setValue(value)
        }
    },
}

