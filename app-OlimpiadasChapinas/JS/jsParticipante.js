function validarGuardadoDeCampos(event) {
    event.preventDefault();

    let respuesta = "";
    if ($("#idPais").val() == null) {
        respuesta += "\n{ID del País}";
    }

    if ($("#idUsuario").val() == null) {
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

    if ($("#genero").val() == null) {
        respuesta += "\n{Genero}";
    }

    if (respuesta !== "") {
        alert(`Los siguientes campos no pueden quedar vacíos: ${respuesta}`);
    } else {
        $("form").submit(); // Envía el formulario si no hay errores
    }
}

const input = document.getElementById("idParticipante");

if (input) {
    const mensaje = "Escribe el ID del Participante";
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