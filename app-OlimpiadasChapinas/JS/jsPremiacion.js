function validarGuardadoDeCampos(event) {
    event.preventDefault();

    let respuesta = "";
    if ($("#idEvento").val().trim() == "") {
        respuesta += "\n{ID del evento}";
    }

    if ($("#idPuesto").val().trim() == "") {
        respuesta += "\n{ID del puesto de premiación}";
    }

    if ($("#idParticipante").val().trim() == "") {
        respuesta += "\n{ID del participante}";
    }

    if (respuesta !== "") {
        alert(`Los siguientes campos no pueden quedar vacíos: ${respuesta}`);
    } else {
        $("form").submit(); // Envía el formulario si no hay errores
    }
}