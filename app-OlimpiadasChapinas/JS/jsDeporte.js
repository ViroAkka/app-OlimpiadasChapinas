function validarGuardadoDeCampos(event) {
    event.preventDefault();

    let respuesta = "";

    if ($("#nombre").val().trim() == "") {
        respuesta += "\n{nombre}";
    }

    if ($("#categoria").val().trim() == "") {
        respuesta += "\n{categoria}";
    }

    if ($("#descripcion").val().trim() == "") {
        respuesta += "\n{descripcion}";
    }

    if ($("#cantidadJugadores").val().trim() == "") {
        respuesta += "\n{cantidadJugadores}";
    }

    if (respuesta !== "") {
        alert(`Los siguientes campos no pueden quedar vacíos: ${respuesta}`);
    } else {
        $("form").submit(); // Envía el formulario si no hay errores
    }
}