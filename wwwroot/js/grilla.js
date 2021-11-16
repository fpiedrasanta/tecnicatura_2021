function crearGrilla(id, url, entidad, columnas, funcionFiltro, busqueda) {
    $(id).on('error.dt', function (e, settings, techNote, message) {
        console.log(message);
        msjError("Error", "Se generó un error mientras intentabamos listar las/os " + entidad + ".");
    }).DataTable({
        language: {
            "processing": `<div class="spinner-border text-primary" role="status">
                            <span class= "sr-only" >Cargando..</span >
                           </div>`,
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": `<div class="spinner-border text-primary" role="status">
                            <span class= "sr-only" >Cargando..</span >
                           </div>`,
            "oPaginate": {
                "sFirst": "<<",
                "sLast": ">>",
                "sNext": ">",
                "sPrevious": "<"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            },
            "buttons": {
                "copy": "Copiar",
                "colvis": "Visibilidad"
            }
        },
        "responsive": true,
        "processing": true,
        "serverSide": true,
        "destroy": true,
        "info": true,
        "searching": busqueda == null ? true : busqueda,
        "paging": true,
        "pagingType": "full_numbers",
        "sort": true,
        "lengthMenu": [[10, 20, 50], [10, 20, 50]],
        "ajax": {
            "url": urlContent + url,
            "type": "POST",
            dataFilter: function (data) {
                var json = JSON.parse(data);

                var error = false;
                if (json.tipoError == 2) {
                    window.location = urlContent + json.mensajeError;
                }

                if (!json.success) {
                    /* Error */
                    var msj = "";
                    var title = "";
                    if (json.innerObject != null && json.innerObject != 'undefined') {
                        var item = 0;

                        for (item in json.innerObject) {
                            msj += json.innerObject[item] + "<br />";
                        }

                        title = json.mensajeError;
                    }
                    else {
                        msj = json.mensajeError;
                        title = "Error";
                    }

                    msjError(title, msj);

                    error = true;
                }

                json.data = error ? [] : json.innerObject.data;
                json.draw = error ? null : json.innerObject.draw;
                json.recordsTotal = error ? 0 : json.innerObject.recordsTotal;
                json.recordsFiltered = error ? 0 : json.innerObject.recordsTotal;
                return JSON.stringify(json);
            },
            "data": function (d) {
                var extraParams = {}
                if (funcionFiltro != null) {
                    extraParams = funcionFiltro();
                }
                return $.extend({}, d, extraParams);
            }
        },
        "columns": columnas
    });
}

function ActualizarGrilla(id, reload = false) {
    if (reload) {
        $(id).DataTable().ajax.reload(null, false);
    } else {
        $(id).DataTable().page(0).draw();
    }
}