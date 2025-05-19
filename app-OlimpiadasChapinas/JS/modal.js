function mostrarModalEliminar(url) {
    const btn = document.getElementById('btnConfirmarEliminar');
    btn.setAttribute('href', url);
    const modal = new bootstrap.Modal(document.getElementById('confirmarEliminarModal'));
    modal.show();
}
