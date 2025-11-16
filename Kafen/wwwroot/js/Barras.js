// Create the chart
$(document).ready(function () {
    $.ajax({
        type: "GET",
        contentType: "application/json;charset-utf-8",
        dataType: "json",
        url: "http://" + host + "/Vendedor/DataBarras",
        error: function () {

            alert("Ocurrio un error al consultar los datos");
        },
        success: function (data) {
            console.log(data);
            GraficaBarras(data);
        }



    })
})
function GraficaBarras(data) {
    Highcharts.chart('top10', {
        chart: {
            type: 'column'
        },
        title: {
            text: 'Ventas de la semana'
        },
        subtitle: {
            text: 'La venta de los ultimos 5 dias'
        },
        xAxis: {
            type: 'category',
            labels: {
                rotation: -45,
                style: {
                    fontSize: '13px',
                    fontFamily: 'Verdana, sans-serif'
                }
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Pedidos entregados'
            }
        },
        legend: {
            enabled: false
        },
        tooltip: {
            pointFormat: 'Cantidad vendida neta: <b>{point.y:.1f} pedidos</b>'
        },
        series: [{
            name: 'Population',
            data: data,
            dataLabels: {
                enabled: true,
                rotation: -90,
                color: '#FFFFFF',
                align: 'right',
                format: '{point.y:.1f}', // one decimal
                y: 10, // 10 pixels down from the top
                style: {
                    fontSize: '13px',
                    fontFamily: 'Verdana, sans-serif'
                }
            }
        }]
    });
}