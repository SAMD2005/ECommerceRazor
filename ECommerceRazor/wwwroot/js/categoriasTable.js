  $(document).ready(function(){
        $("#categoriasTable").DataTable({
            language: {
                url: "//cdn.datatables.net/plug-ins/1.13.5/i18n/es-ES.json" //Traduccion al español 
            },
            pageLength: 10, //Numero de filas por pagina
            ordering: true, //habilitar ordenamiento
            searching: true, //Habilitar busqueda 
            columnDefs: [
                { orderable: false, targets: 4 } // Deshabilitar ordenamiento en la columna de imagenes 
            ]
        });
        });

