$(document).ready(
    function () {
        loadCustomerAndBookingLineChart();
    }
)

function loadCustomerAndBookingLineChart() {
    $(".chart-spinner").show();

    $.ajax({
        url: "/Dashboard/GetMemberAndBookingLineChartData",
        type: "Get",
        dataType: "json",
        success: function (data) {

            loadLineChart("NewMembersAndBookingsLineChart", data);

            $(".chart-spinner").hide();
        }

    });  
}

function loadLineChart(id, data) {
    var chartColors = getCharColorsArray(id);
    options = {
        colors: chartColors,
        series: data.series,
        chart: {
            type: "line",
            width: 770,
            height : 300
        },
        stroke: {
            curve: "smooth",
            width: 2
        },
        markers: {
            size: 3,
            strokeWidth : 0,
            hover: {
                size: 7
            }
        },
        xaxis: {
            categories: data.categories,
            labels: {
                style: {
                    colors: "#DDD"
                }
            }
        },
        yaxis:
        {
            labels: {
                style: {
                    colors: "#FFF"
                }
            }
        },
        legend: {
            labels: {
                colors : "#FFF"
            }
        },
        tooltip: {
            theme : "dark"
        }
        }

    var chart = new ApexCharts(document.querySelector("#" + id), options);
    chart.render();
    }