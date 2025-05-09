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

const input = document.getElementById("idEvento");

if (input) {
    const mensaje = "Escribe el ID del Deporte";
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