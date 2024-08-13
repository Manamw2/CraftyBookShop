var dataTable;

$(document).ready(() => {
    loadTableData(x);
});

function loadTableData(status) {
    $('#myTable').DataTable().destroy();
    dataTable = $('#myTable').DataTable({
        "ajax": { url: '/order/getall?status=' + status },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "20%" },
            { data: 'phoneNumber', "width": "15%" },
            { data: 'appUser.email', "width": "25%" },
            { data: 'orderStatus', "width": "15%" },
            { data: 'totalPrice', "width": "10%" },
            {
                data: 'id', "width": "10%", "render": function (data) {
                    return `<div class="input-group" role="group">
                                <a href="/order/details?id=${data}" class="btn btn-primary mx-2">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                            </div>`
                }
            }]
    });
}
