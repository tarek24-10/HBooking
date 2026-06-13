$(document).ready(
    function () {
        loadTotalUserRadialChart();
    }
)

function loadTotalUserRadialChart() {
    $(".chart-spinner").show();

    $.ajax({
        url: "/Dashboard/GetTotalRegistrationRadialChartData",
        type: "Get",
        dataType: "json",
        success: function (data) {

            $("#totalUserCount").html(data.totalCount);

            if (data.hasRatioIncreased) {
                $("#sectionUserCount").html(`<span class="text-success">
                    <i class="bi bi-arrow-up-right-circle me-1 ms-1"></i>
                </span> ${data.countInCurrentMonth}`)
            }

            else {
                $("#sectionUserCount").html(`<span class="text-danger">
                    <i class="bi bi-arrow-down-right-circle me-1 ms-1"></i>
                </span> ${data.countInCurrentMonth}`)
            }

            loadRadialBarChart("totalUserRadialChart", data);

            $(".chart-spinner").hide();
        }

    });  
}