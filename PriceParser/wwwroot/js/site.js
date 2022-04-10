// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

google.charts.load('current', { 'packages': ['line'] });
//google.charts.setOnLoadCallback(reloadChart);

async function reloadChart() {

    var startPeriod = document.getElementById('startPeriod'); 
    var endPeriod = document.getElementById('endPeriod');
    var entityId = document.getElementById('entityId');
    var entityType = document.getElementById('entityType');
    var chartElement = document.getElementById('pricesChart');

    if (startPeriod === null || endPeriod === null || entityId === null || entityType === null) {
        return;
    }

    console.log(entityId.value);
    console.log(endPeriod.value);
    console.log(startPeriod.value);
    console.log(entityType.value);

    if (startPeriod.value === "" || endPeriod.value === "" || entityId.value === "" || entityType.value === "") {
        return;
    }

    let url = `/${entityType.value}/PricesData?id=${entityId.value}&startPeriod=${startPeriod.value}&endPeriod=${endPeriod.value}`;

    console.log(url);

    var chartData = await fetch(url)
        .then((response) => {
            return response.json();
        });

    var dates = [];

    for (var i in chartData) {

        for (var j in chartData[i].prices) {
            let dateEl = chartData[i].prices[j].date;

            if (!dates.includes(dateEl)) {
                dates.push(dateEl);
            }
        }           
    }

    dates.sort((a, b) => new Date(a + '+0300') - new Date(b + '+0300'));

    var data = new google.visualization.DataTable();
    data.addColumn('datetime', 'Day');

    for (var i = 0; i < chartData.length; i++) {
        data.addColumn('number', chartData[i].site_name);
    }

    var dataArray = [];

    for (var dateIndex in dates) {

        var dataArrayElement = [];
        dataArrayElement.push(new Date(dates[dateIndex] + '+0300'));

        for (var i in chartData) {

            let priceEl = chartData[i].prices.find(item => item.date == dates[dateIndex]);

            if (priceEl != undefined) {
                dataArrayElement.push(priceEl.price);
            }
            else {
                dataArrayElement.push(0);
            }

        }

        dataArray.push(dataArrayElement);

    }   


    //for (var i in chartData) {
    //    dataArray.push([new Date(chartData[i].date+'+0300'), chartData[i].price]);
    //}

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
        },
        vAxis: {
            format: '####.##',
            title: 'BYN'
        }
    };

    var formatter = new google.visualization.NumberFormat(
        { prefix: 'BYN ', negativeColor: 'red', negativeParens: true });
    formatter.format(data, 1);

    var chart = new google.charts.Line(chartElement);

    chart.draw(data, google.charts.Line.convertOptions(options));
}