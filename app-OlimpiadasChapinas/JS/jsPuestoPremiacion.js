function validarGuardadoDeCampos(event) {
    event.preventDefault();

    let respuesta = "";
    if ($("#idPuesto").val().trim() == "") {
        respuesta += "\n{ID del Puesto}";
    }

    if ($("#descripcion").val().trim() == "") {
        respuesta += "\n{Descripción del puesto}";
    }

    if (respuesta !== "") {
        alert(`Los siguientes campos no pueden quedar vacíos: ${respuesta}`);
    } else {
        $("form").submit(); // Envía el formulario si no hay errores
    }
}