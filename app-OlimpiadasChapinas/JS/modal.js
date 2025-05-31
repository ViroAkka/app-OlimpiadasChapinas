function mostrarModalEliminar(url) {
    const btn = document.getElementById('btnConfirmarEliminar');
    btn.setAttribute('href', url);
    const modal = new bootstrap.Modal(document.getElementById('confirmarEliminarModal'));
    modal.show();
}

function mostrarNotificacion(mensaje) {
    $("#mensajeNotificacion").text(mensaje);
    let modal = new bootstrap.Modal(document.getElementById('notificacionModal'));
    modal.show();

    setTimeout(() => {
        modal.hide();
    }, 2000); 
}