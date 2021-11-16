function msjError(title, msj, funCerrar) {
    agregarTexto('modal-danger', title, msj, 'cil-x-circle');
    $('#btn_aceptar').hide();
    $('#btn_cerrar').show();
    $('#btn_cerrar').on('click', function () {
        $('#btn_cerrar').hide();
        cerrarModal(funCerrar);
    });
}

function msjSuccess(title, msj, funCerrar) {
    agregarTexto('modal-success', title, msj, 'cil-check-circle');
    $('#btn_aceptar').hide();
    $('#btn_cerrar').show();
    $('#btn_cerrar').on('click', function () {
        $('#btn_cerrar').hide();
        cerrarModal(funCerrar);
    });
}

function msjInfo(title, msj) {
    agregarTexto('modal-info', title, msj, 'cil-lightbulb');
    $('#btn_aceptar').hide();
    $('#btn_cerrar').show();
}

function msjPregunta(title, msj, funAceptar, funCerrar) {
    agregarTexto('modal-danger', title, msj, 'cil-warning');
    $('#btn_aceptar').show();
    $('#btn_cerrar').show();

    $('#btn_aceptar').on('click', function () {
        $('#btn_aceptar').hide();
        aceptarModal(funAceptar);
    });
    $('#btn_cerrar').on('click', function () {
        $('#btn_cerrar').hide();
        cerrarModal(funCerrar);
    });
}

function msjWarning(title, msj, funCerrar) {
    agregarTexto('modal-danger', title, msj, 'cil-warning');
    $('#btn_aceptar').hide(); 
    $('#btn_cerrar').show();
    $('#btn_cerrar').on('click', function () {
        $('#btn_cerrar').hide();
        cerrarModal(funCerrar);
    });
}

function agregarTexto(modaltype, title, msj, icon) {
    jQuery.noConflict();
    $('#btn_aceptar').hide();
    $('#btn_cerrar').hide();
    $('#btn_aceptar').off();
    $('#btn_cerrar').off();
    $('#contenedor_modal').removeClass().addClass(modaltype);
    $('#titulo_modal').html(title);
    $('#texto_modal span').html(msj);
    $('#icono_modal').html('<use xlink:href="/img/icons/svg/free.svg#' + icon +'"></use>');
    $('#small_modal').modal('show');
}

function cerrarModal(funCerrar) {
    $('#small_modal').modal('hide');
    if (funCerrar != null) {
        funCerrar();
    }
}

function aceptarModal(funAceptar) {
    $('#small_modal').modal('hide');
    if (funAceptar != null) {
        funAceptar();
    }
}