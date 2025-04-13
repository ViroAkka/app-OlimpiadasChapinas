function validarGuardadoDeCampos(event) {
    event.preventDefault();

    let respuesta = "";
    if ($("#idEvento").val().trim() == "") {
        respuesta += "\n{ID del Evento}";
    }

    if ($("#idParticipante").val().trim() == "") {
        respuesta += "\n{ID del Participnate}";
    }

    if ($("#idPago").val().trim() == "") {
        respuesta += "\n{ID del Pago}";
    }

    if ($("#fuentePublicidad").val().trim() == "") {
        respuesta += "\n{Fuente de Publicidad donde se enteró del evento}";
    }

    if (respuesta !== "") {
        alert(`Los siguientes campos no pueden quedar vacíos: ${respuesta}`);
    } else {
        $("form").submit(); // Envía el formulario si no hay errores
    }
}