function validarGuardadoDeCampos(event) {
    event.preventDefault();

    let respuesta = "";
    if ($("#idFormaPago").val().trim() == "") {
        respuesta += "\n{ID de la Forma de Pago}";
    }

    if ($("#montoPago").val().trim() == "") {
        respuesta += "\n{Monto del Pago}";
    }

    if (respuesta !== "") {
        alert(`Los siguientes campos no pueden quedar vacíos: ${respuesta}`);
    } else {
        $("form").submit(); // Envía el formulario si no hay errores
    }
}