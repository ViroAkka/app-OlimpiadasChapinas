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

const inputUsuario = document.getElementById("idUsuario");
const inputEmail = document.getElementById("email");

if (inputUsuario && inputEmail) {
    const mensajeUsuario= "Escribe el ID del Usuario";
    const mensajeEmail = "Escribe el Email del Usuario";

    let indexUsuario= 0;
    let borrandoUsuario= false;

    let indexEmail = 0;
    let borrandoEmail = false;

    function animarPlaceholderUsuario() {
        if (!borrandoUsuario) {
            inputUsuario.placeholder = mensajeUsuario.substring(0, indexUsuario+ 1);
            indexUsuario++;
            if (indexUsuario=== mensajeUsuario.length) {
                borrandoUsuario= true;
                setTimeout(animarPlaceholderUsuario, 2000);
                return;
            }
        } else {
            inputUsuario.placeholder = mensajeUsuario.substring(0, indexUsuario- 1);
            indexUsuario--;
            if (indexUsuario=== 0) {
                borrandoUsuario= false;
            }
        }
        setTimeout(animarPlaceholderUsuario, 70);
    }

    function animarPlaceholderEmail() {
        if (!borrandoEmail) {
            inputEmail.placeholder = mensajeEmail.substring(0, indexEmail + 1);
            indexEmail++;
            if (indexEmail === mensajeEmail.length) {
                borrandoEmail = true;
                setTimeout(animarPlaceholderEmail, 1400);
                return;
            }
        } else {
            inputEmail.placeholder = mensajeEmail.substring(0, indexEmail - 1);
            indexEmail--;
            if (indexEmail === 0) {
                borrandoEmail = false;
            }
        }
        setTimeout(animarPlaceholderEmail, 70);
    }

    animarPlaceholderUsuario();
    animarPlaceholderEmail();
}