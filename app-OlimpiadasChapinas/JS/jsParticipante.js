function validarGuardadoDeCampos(event) {
    event.preventDefault();

    let respuesta = "";
    if ($("#idPais").val().trim() == "") {
        respuesta += "\n{ID del País}";
    }

    if ($("#idUsuario").val().trim() == "") {
        respuesta += "\n{ID del Usuario}";
    }

    if ($("#fechaNacimiento").val().trim() == "") {
        respuesta += "\n{Fecha de Nacimiento}";
    }

    if ($("#altura").val().trim() == "") {
        respuesta += "\n{Altura}";
    }

    if ($("#peso").val().trim() == "") {
        respuesta += "\n{Peso}";
    }

    if ($("#genero").val().trim() == "" || $("#genero").val() == null) {
        respuesta += "\n{Genero}";
    }

    if (respuesta !== "") {
        alert(`Los siguientes campos no pueden quedar vacíos: ${respuesta}`);
    } else {
        $("form").submit(); // Envía el formulario si no hay errores
    }
}