if (typeof (Logistics) === "undefined") Logistics = {};
if (typeof (Logistics.OportunidadeAmbiente) === "undefined") Logistics.OportunidadeAmbiente = {};



Logistics.OportunidadeAmbiente = {
    AmbientedaOportunidade: function (context) {
        let formContext = context.getFormContext();
        formContext.getAttribute("alfa_criadonodynamics1").setValue(true);
    }
};