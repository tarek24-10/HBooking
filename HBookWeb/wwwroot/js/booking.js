var dataTable;

$(document).ready(function () {
    loadDataatble();
});
function loadDataatble() {
    $("#tblBookings").DataTable({
        "ajax": {
            "url": "/Booking/GetAll"
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