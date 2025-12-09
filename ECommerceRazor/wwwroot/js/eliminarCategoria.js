       document.querySelectorAll('.btn-delete').forEach(button => {
        button.addEventListener('click', function () {
            const productoId = this.getAttribute('data-id');
            const productoName = this.getAttribute('data-name');

            Swal.fire({
                title: `¿Estás seguro de eliminar la categoría "${productoName}"?`,
                text: "Esta acción no se puede deshacer.",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#d33",
                cancelButtonColor: "#3085d6",
                confirmButtonText: "Sí, eliminar",
                cancelButtonText: "Cancelar"
            }).then((result) => {
                if (result.isConfirmed) {
                    //console.log(JSON.stringify(categoriaId));
                    fetch("?handler=Delete", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json",
                            "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
                        },
                        body: JSON.stringify(productoId) // Asegúrate de enviar el id como un número

                    })
                        .then(response => response.json())
                        .then(data => {
                            if (data.success) {
                                Swal.fire(
                                    "Eliminado",
                                    `El producto "${productoName}" ha sido eliminada.`,
                                    "success"
                                ).then(() => {
                                    window.location.reload();
                                });
                            } else {
                                Swal.fire(
                                    "Error",
                                    data.message || "Ocurrió un problema al eliminar la categoría.",
                                    "error"
                                );
                            }
                        });
                }
            });
        });
        });
