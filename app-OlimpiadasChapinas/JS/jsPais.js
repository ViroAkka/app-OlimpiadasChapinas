function validarGuardadoDeCampos(event) {
    event.preventDefault();

    let respuesta = "";
    if ($("#idPais").val().trim() == "") {
        respuesta += "\n{ID del Pais}";
    }

    if ($("#nombre").val().trim() == "") {
        respuesta += "\n{Nombre del País}";
    }

    if (respuesta !== "") {
        alert(`Los siguientes campos no pueden quedar vacíos: ${respuesta}`);
    } else {
        $("form").submit(); // Envía el formulario si no hay errores
    }
}