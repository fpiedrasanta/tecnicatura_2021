function ajax(type, url, jsonData, msgAjax, msgProgreso, functionSuccess, functionError) {
    ajaxShowBack(msgAjax, msgProgreso);
    $.ajax({
        type: type,
        url: url,
        data: jsonData,
        success: function (retorno) {
            ajaxHideBack();
            procesarRespuesta(retorno, functionSuccess, functionError);
        },
        error: function (objeto, error, objeto2) {
            ajaxHideBack();
            msjError("Error", objeto.responseText);
        }
    });
}

function ajaxShowBack(msgAjax, msgProgreso) {
    $.blockUI({
        baseZ: 5000,
        blockMsgClass: 'alertBox',
        message: `<div class="spinner-border text-primary" role="status">
                    <span class= "sr-only" > ${msgProgreso}</span>
                    </div>
    <h5>${msgAjax}</h5>` });
}

function ajaxHideBack() {
    $.unblockUI();
}

function procesarRespuesta(retorno, functionSuccess, functionError) {
    if (retorno.tipoError == 2) {
        window.location = urlContent + retorno.mensajeError;
    }
    else {
        if (retorno.success) {
            functionSuccess(retorno);
        } else {
            /* Error */
            var msj = "";
            var title = "";
            if (retorno.innerObject != null && retorno.innerObject != 'undefined') {
                var item = 0;

                for (item in retorno.innerObject) {
                    msj += retorno.innerObject[item] + "<br />";
                }

                title = retorno.mensajeError;
            } else {
                msj = retorno.mensajeError;
                title = "Error";
            }

            if (functionError == null || functionError == 'undefined') {
                msjError(title, msj);
            } else {
                functionError(msj, title);
            }
        }
    }
}