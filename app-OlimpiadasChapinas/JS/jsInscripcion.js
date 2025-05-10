function validarGuardadoDeCampos(event) {
    event.preventDefault();

    let respuesta = "";
    if ($("#idEvento").val() == null) {
        respuesta += "\n{ID del Evento}";
    }

    if ($("#idParticipante").val() == null){
        respuesta += "\n{ID del Participnate}";
    }

    if ($("#idPago").val() == null) {
        respuesta += "\n{ID del Pago}";
    }

    if ($("#fuentePublicidad").val().trim() == "") {
        respuesta += "\n{Fuente de Publicidad donde se enteró del evento}";
    }

    if (respuesta !== "") {
        alert(`Los siguientes campos no pueden quedar vacíos: ${respuesta}`);
    } else {
        $("form").submit(); // Envía el formulario si no hay errores
    }
}

const inputEvento = document.getElementById("idEvento");
const inputParticipante = document.getElementById("idParticipante");
const inputPago = document.getElementById("idPago");

if (inputEvento && inputParticipante && inputPago) {
    const mensajeEvento = "Escribe el ID del Evento";
    const mensajeParticipante = "Escribe el ID del Participante";
    const mensajePago = "Escribe el ID del Pago";

    let indexEvento = 0;
    let borrandoEvento = false;

    let indexParticipante = 0;
    let borrandoParticipante = false;

    let indexPago = 0;
    let borrandoPago = false;

    function animarPlaceholderEvento() {
        if (!borrandoEvento) {
            inputEvento.placeholder = mensajeEvento.substring(0, indexEvento + 1);
            indexEvento++;
            if (indexEvento === mensajeEvento.length) {
                borrandoEvento = true;
                setTimeout(animarPlaceholderEvento, 2000);
                return;
            }
        } else {
            inputEvento.placeholder = mensajeEvento.substring(0, indexEvento - 1);
            indexEvento--;
            if (indexEvento === 0) {
                borrandoEvento = false;
            }
        }
        setTimeout(animarPlaceholderEvento, 70);
    }

    function animarPlaceholderParticipante() {
        if (!borrandoParticipante) {
            inputParticipante.placeholder = mensajeParticipante.substring(0, indexParticipante + 1);
            indexParticipante++;
            if (indexParticipante === mensajeParticipante.length) {
                borrandoParticipante = true;
                setTimeout(animarPlaceholderParticipante, 1400);
                return;
            }
        } else {
            inputParticipante.placeholder = mensajeParticipante.substring(0, indexParticipante - 1);
            indexParticipante--;
            if (indexParticipante === 0) {
                borrandoParticipante = false;
            }
        }
        setTimeout(animarPlaceholderParticipante, 70);
    }

    function animarPlaceholderPago() {
        if (!borrandoPago) {
            inputPago.placeholder = mensajePago.substring(0, indexPago + 1);
            indexPago++;
            if (indexPago === mensajePago.length) {
                borrandoPago = true;
                setTimeout(animarPlaceholderPago, 2000);
                return;
            }
        } else {
            inputPago.placeholder = mensajePago.substring(0, indexPago - 1);
            indexPago--;
            if (indexPago === 0) {
                borrandoPago = false;
            }
        }
        setTimeout(animarPlaceholderPago, 70);
    }

    animarPlaceholderEvento();
    animarPlaceholderParticipante();
    animarPlaceholderPago();
}