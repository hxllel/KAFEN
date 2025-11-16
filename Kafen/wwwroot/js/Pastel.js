
$(document).ready(function () {
    $.ajax({
        type: "GET",
        contentType: "application/json;charset-utf-8",
        dataType: "json",
        url: "http://"+host+"/Vendedor/DataPastel",
        error: function (){

            alert ("Ocurrio un error al consultar los datos");
        },
        success: function (data) {
            console.log(data);
            GraficaPastel(data);
        }
            
    

    })
})

function GraficaPastel(data) {
    Highcharts.chart('categorias', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: 'Categorias Mas Vendidas'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                }
            }
        },
        series: [{
            name: 'Categoria',
            colorByPoint: true,
            data: data
        }]
    });
} 