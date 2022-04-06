// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

google.charts.load('current', { 'packages': ['line'] });
google.charts.setOnLoadCallback(reloadChart);

async function reloadChart() {

    var startPeriod = document.getElementById('startPeriod');
    console.log(startPeriod.value);

    var endPeriod = document.getElementById('endPeriod');
    console.log(endPeriod.value);

    var entityId = document.getElementById('entityId');
    console.log(entityId.value);

    if (startPeriod.value === "" || endPeriod.value === "" || entityId.value === "") {
        return;
    }

    let url = `/ProductsFromSites/PricesData?id=${entityId.value}&startPeriod=${startPeriod.value}&endPeriod=${endPeriod.value}`;

    console.log(url);

    var chartData = await fetch(url)
        .then((response) => {
            return response.json();
        });

    var labelsRange = [];

    for (var i in chartData)
        labelsRange.push(chartData[i].date);

    var dataRange = [];

    for (var i in chartData)
        dataRange.push(chartData[i].price);


    var data = new google.visualization.DataTable();
    data.addColumn('datetime', 'Day');
    data.addColumn('number', 'Price');
    /*data.addColumn('number', 'The Avengers');
    data.addColumn('number', 'Transformers: Age of Extinction');*/

    var dataArray = [];


    for (var i in chartData) {
        dataArray.push([new Date(chartData[i].date+'+0300'), chartData[i].price]);
    }

    data.addRows(dataArray);
    //data.addRows([
    //    [1, 37.8, 80.8, 41.8],
    //    [2, 30.9, 69.5, 32.4],
    //    [3, 25.4, 57, 25.7],
    //    [4, 11.7, 18.8, 10.5],
    //    [5, 11.9, 17.6, 10.4],
    //    [6, 8.8, 13.6, 7.7],
    //    [7, 7.6, 12.3, 9.6],
    //    [8, 12.3, 29.2, 10.6],
    //    [9, 16.9, 42.9, 14.8],
    //    [10, 12.8, 30.9, 11.6],
    //    [11, 5.3, 7.9, 4.7],
    //    [12, 6.6, 8.4, 5.2],
    //    [13, 4.8, 6.3, 3.6],
    //    [14, 4.2, 6.2, 3.4]
    //]);

    var options = {
        chart: {
            title: 'Price dynamics',
            subtitle: 'in BYN'
        },
        width: 900,
        height: 500,
        hAxis: {
            format: 'dd.MM.yyyy'
        }
    };

    var formatter = new google.visualization.NumberFormat(
        { prefix: 'BYN ', negativeColor: 'red', negativeParens: true });
    formatter.format(data, 1);

    var chart = new google.charts.Line(document.getElementById('pricesChart'));

    chart.draw(data, google.charts.Line.convertOptions(options));
}