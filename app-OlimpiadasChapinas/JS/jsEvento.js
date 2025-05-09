function validarGuardadoDeCampos(event) {
    event.preventDefault();

    let respuesta = "";
    if ($("#nombre").val().trim() == "") {
        respuesta += "\n{Nombre del Evento}";
    }

    if ($("#idDeporte").val() == null) {
        respuesta += "\n{ID del Deporte}";
    }

    if ($("#idEventoPadre").val() == null) {
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

const input = document.getElementById("idEvento");

if (input) {
    const mensaje = "Escribe el ID del Evento";
    let index = 0;
    let borrando = false;

    function animarPlaceholder() {
        if (!borrando) {
            input.placeholder = mensaje.substring(0, index + 1);
            index++;
            if (index === mensaje.length) {
                borrando = true;
                setTimeout(animarPlaceholder, 2000);
                return;
            }
        } else {
            input.placeholder = mensaje.substring(0, index - 1);
            index--;
            if (index === 0) {
                borrando = false;
            }
        }
        setTimeout(animarPlaceholder, 70);
    }

    animarPlaceholder();
}