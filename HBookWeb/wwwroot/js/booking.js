var dataTable;

$(document).ready(function () {
    var urlParams = new URLSearchParams(window.location.search);
    var status = urlParams.get('status');
    loadDataatble(status);
});
function loadDataatble(status) {
    $("#tblBookings").DataTable({
        "ajax": {
            "url": "/Booking/GetAll?status=" + status
        },

        "columns": [
            { "data": "id"},
            { "data": "name"},
            { "data": "phoneNumber"},
            { "data": "email"},
            { "data": "status"},
            { "data": "checkInDate"},
            { "data": "nights"},
            { "data": "totalCost", "render": function (data) {
                return `$${data.toFixed(2)}`;
            } },
            { "data": "id", 
                "render": function (data) {
                    return `<div class="w-75 btn-group">
                        <a href="/Booking/BookingDetails?bookingId=${data}" class="btn btn-outline-warning mx-2">
                           <i class="bi bi-pencil-square"></i> Details</a>
                    </div>`
                }
            }
        ]
    });
}