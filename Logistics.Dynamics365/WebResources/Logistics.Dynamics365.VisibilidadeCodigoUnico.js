if (typeof (Logistics) === "undefined") Logistics = {};
if (typeof (Logistics.Oportunidade) === "undefined") Logistics.Oportunidade = {};



Logistics.Oportunidade = {
    VisibilidadeCodigoAlfanumerico: function (context) {
        let formContext = context.getFormContext();
        formContext.getControl("alfa_codigounico").setVisible(true);
        formContext.getControl("alfa_codigounico").setDisabled(true);
    }
};