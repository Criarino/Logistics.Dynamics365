if (typeof (Logistics) === "undefined") Logistics = {};
if (typeof (Logistics.OportunidadeEditavel) === "undefined") Logistics.OportunidadeEditavel = {};

Logistics.OportunidadeEditavel = {
    OnLoad: function (context) {
        var formContext = context.getFormContext();
        var feitoPorDynamics1 = formContext.getAttribute("alfa_criadonodynamics1").getValue();
        console.log("Foi onloadado");
        if (feitoPorDynamics1 == true) {
            // Desativar todos os controls no form
            formContext.ui.controls.forEach((control) => {
                control.setDisabled(true);
                console.log("Deveria ter sido desativado");
            });

            formContext.ui.setFormNotification("Essa Oportunidade nao pode ser editada, pois foi criada pelo Dynamics1", "INFO");
        }
    }
}
