function validarGuardadoDeCampos(event) {
    event.preventDefault();

    let respuesta = "";
    if ($("#nombre").val().trim() == "") {
        respuesta += "\n{Nombre del Evento}";
    }

    if ($("#idDeporte").val().trim() == "") {
        respuesta += "\n{ID del Deporte}";
    }

    if ($("#idEventoPadre").val().trim() == "") {
        respuesta += "\n{ID del Evento Principal}";
    }

    if ($("#fechaInicio").val().trim() == "") {
        respuesta += "\n{Fecha de Inicio}";
    }

    if ($("#fechaFin").val().trim() == "") {
        respuesta += "\n{Fecha de Fin}";
    }

    if ($("#cantidadParticipantes").val().trim() == "") {
        respuesta += "\n{Cantidad de Participantes}";
    }

    if ($("#montoInscripcion").val().trim() == "") {
        respuesta += "\n{Monto de Inscripción}";
    }

    if (respuesta !== "") {
        alert(`Los siguientes campos no pueden quedar vacíos: ${respuesta}`);
    } else {
        $("form").submit(); // Envía el formulario si no hay errores
    }
}