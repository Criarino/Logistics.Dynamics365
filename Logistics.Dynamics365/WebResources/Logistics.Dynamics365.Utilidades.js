if (typeof (Logistics) === "undefined") Logistics = {};
if (typeof (Logistics.Utilidades) === "undefined") Logistics.Utilidades = {};


Logistics.Utilidades = {

    Alerta: function (titulo, texto) {

        let conteudo = {

            confirmButtonLabel: "OK",
            title: titulo,
            text: texto

        }

        let dialogConfig = {

            height: 120,
            width: 200

        }

        Xrm.Navigation.openAlertDialog(conteudo, dialogConfig)



    }
}