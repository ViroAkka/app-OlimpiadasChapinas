function validarGuardadoDeCampos(event) {
    event.preventDefault();

    let respuesta = "";
    if ($("#nombre").val().trim() == "") {
        respuesta += "\n{Nombre}";
    }

    if ($("#apellido").val().trim() == "") {
        respuesta += "\n{Apellido}";
    }

    if ($("#email").val().trim() == "") {
        respuesta += "\n{Email}";
    }

    if ($("#contraseña_hash").val().trim() == "") {
        respuesta += "\n{Contraseña del usuario}";
    }

    if ($("#telefono").val().trim() == "") {
        respuesta += "\n{Teléfono}";
    }

    if ($("#DNI").val().trim() == "") {
        respuesta += "\n{DNI}";
    }

    if (respuesta !== "") {
        alert(`Los siguientes campos no pueden quedar vacíos: ${respuesta}`);
    } else {
        $("form").submit(); // Envía el formulario si no hay errores
    }
}

function validarGuardadoDeCamposActualizacion(event) {
    event.preventDefault();

    let respuesta = "";
    if ($("#nombre").val().trim() == "") {
        respuesta += "\n{Nombre}";
    }

    if ($("#apellido").val().trim() == "") {
        respuesta += "\n{Apellido}";
    }

    if ($("#email").val().trim() == "") {
        respuesta += "\n{Email}";
    }

    if ($("#contraseñaAlmacenada").val().trim() == "") {
        respuesta += "\n{Antigua contraseña del usuario}";
    }

    if ($("#contraseñaActualizada").val().trim() == "") {
        respuesta += "\n{Nueva contraseña del usuario}";
    }

    if ($("#telefono").val().trim() == "") {
        respuesta += "\n{Teléfono}";
    }

    if (respuesta !== "") {
        alert(`Los siguientes campos no pueden quedar vacíos: ${respuesta}`);
    } else {
        $("form").submit(); // Envía el formulario si no hay errores
    }
}