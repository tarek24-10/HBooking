$(document).ready(
    function () {
        loadCustomerBookingPieChart();
    }
)

function loadCustomerBookingPieChart() {
    $(".chart-spinner").show();

    $.ajax({
        url: "/Dashboard/GetBookingPieChartData",
        type: "Get",
        dataType: "json",
        success: function (data) {

            loadPieChart("CustomerBookingsPieChart", data);

            $(".chart-spinner").hide();
        }

    });  
}

function loadPieChart(id,data) {
    var chartColors = getChartColorsArray(id);

    options = {
        series: data.series,
        labels: data.labels,
        colors: chartColors,
        chart: {
            type: "pie",
            width: 380
        },
        stroke: {
            show: false
        },
        legend: {
            position: "bottom",
            horizontalAlign: "center",
            labels: {
                colors: "#FFF",
                useSeriesColors : "true"
            }
        }
    }

    var chart = new ApexCharts(document.querySelector("#" + id), options);
    chart.render();
}