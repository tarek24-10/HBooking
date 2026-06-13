$(document).ready(
    function () {
        loadTotalRevenueRadialChart();
    }
)

function loadTotalRevenueRadialChart() {
    $(".chart-spinner").show();

    $.ajax({
        url: "/Dashboard/GetTotalRevenueRadialChartData",
        type: "Get",
        dataType: "json",
        success: function (data) {

            $("#totalRevenueCount").html(data.totalCount);

            if (data.hasRatioIncreased) {
                $("#sectionRevenueCount").html(`<span class="text-success">
                    <i class="bi bi-arrow-up-right-circle me-1 ms-1"></i>
                </span> ${data.countInCurrentMonth}`)
            }

            else {
                $("#sectionRevenueCount").html(`<span class="text-danger">
                    <i class="bi bi-arrow-down-right-circle me-1 ms-1"></i>
                </span> ${data.countInCurrentMonth}`)
            }

            loadRadialBarChart("totalRevenueRadialChart", data);

            $(".chart-spinner").hide();
        }

    });  
}