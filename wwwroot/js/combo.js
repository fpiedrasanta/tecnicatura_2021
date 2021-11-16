function crearCombo(id, url, jsonData) {
    $(id).select2({
        placeholder: "Buscar..",
        ajax: {
            processResults: function (data) {
                var resultado = obtenerResultado(data);
                return {
                    results: resultado ? resultado : []
                };
            },
            transport: function (params, success, failure) {
                var query = params.data.term;
                if (query === null)
                    query = '';
                
                var data = {
                    consulta: query
                };

                var request = $.ajax({
                    type: 'POST',
                    url: url,
                    data: data
                });
                
                request.then(success);
                request.fail(failure);
                
                return request;
            }
        }
    });
}

function crearAutocomplete(id, url, jsonData) {
    $(id).select2({
        placeholder: "Buscar..",
        ajax: {
            processResults: function (data) {
                var resultado = obtenerResultado(data);
                return {
                    results: resultado ? resultado.results : [],
                    pagination: {
                        more: resultado ? resultado.pagination : false
                    }
                };
            },
            transport: function (params, success, failure) {
                console.log(params);
                var query = params.data.term;
                var page = params.data.page;
                if (query === null)
                    query = '';
                if (page == null)
                    page = 0;
                var request = {
                    Query: query,
                    Page: page,
                    PageSize: 10
                };
                jsonData['request'] = request;
                var request = $.ajax({
                    type: 'POST',
                    url: url,
                    async: true,
                    data: JSON.stringify(jsonData),
                    contentType: "application/json",
                    dataType: "json",
                    cache: false,
                    crossDomain: true
                });
                request.then(success);
                request.fail(failure);
                return request;
            }
        }
    });
}

function obtenerResultado(data) {
    if (data.tipoError == 2) {
        window.location = urlContent + retorno.mensajeError;
        return null
    }

    if (data.success) {
        return data.innerObject;
    }
    
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

    msjError(title, msj);
    
    return null;
}

function definirValorCombo(combo,idval, text) {
    $(combo).select2("trigger", "select", {
        data: { id: idval, text: text }
    });
}