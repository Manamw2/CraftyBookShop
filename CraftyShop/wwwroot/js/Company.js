var dataTable;

$(document).ready(() => loadTableData());

function loadTableData() {
    dataTable = $('#myTable').DataTable({
        "ajax": { url: '/company/getall' },
        "columns": [
            { data: 'name', "width": "20%" },
            { data: 'streetAddress', "width": "20%" },
            { data: 'city', "width": "10%" },
            { data: 'state', "width": "10%" },
            { data: 'phoneNumber', "width": "15%" },
            {
                data: 'id', "width": "25%", "render": function (data) {
                    return `<div class="input-group" role="group">
                            <a href="/company/upsert?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a onclick=deleteProduct("/company/delete?id=${data}") class="btn btn-danger mx-2">
                                <i class="bi bi-trash-fill"></i> Delete
                            </a>
                        </div>`
                }
            }]
    });
}

function deleteProduct(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success("Company was deleted successfully");
                }
            });
        }
    });
}