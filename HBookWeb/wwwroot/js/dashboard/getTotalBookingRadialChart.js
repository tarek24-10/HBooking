$(document).ready(
    function () {
        loadTotalBookingRadialChart();
    }
)

function loadTotalBookingRadialChart() {
    $(".chart-spinner").show();

    $.ajax({
        url: "/Dashboard/GetTotalBookingRadialChartData",
        type: "Get",
        dataType: "json",
        success: function (data) {

            $("#totalBookingCount").html(data.totalCount);

            if (data.hasRatioIncreased) {
                $("#sectionBookingCount").html(`<span class="text-success">
                    <i class="bi bi-arrow-up-right-circle me-1 ms-1"></i>
                </span> ${data.countInCurrentMonth}`)
            }

            else {
                $("#sectionBookingCount").html(`<span class="text-danger">
                    <i class="bi bi-arrow-down-right-circle me-1 ms-1"></i>
                </span> ${data.countInCurrentMonth}`)
            }

            loadRadialBarChart("totalBookingRadialChart", data);

            $(".chart-spinner").hide();
        }

    });  
}