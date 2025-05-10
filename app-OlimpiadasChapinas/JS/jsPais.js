function validarGuardadoDeCampos(event) {
    event.preventDefault();

    let respuesta = "";
    if ($("#idPais").val() == null) {
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

const input = document.getElementById("idPais");
const inputNombre = document.getElementById("nombre");

if (input && !inputNombre) {
    const mensaje = "Escribe el ID del País";
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