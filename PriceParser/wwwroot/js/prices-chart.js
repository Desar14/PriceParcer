// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

google.charts.load('current', { 'packages': ['line'] });
//google.charts.setOnLoadCallback(reloadChart);

async function reloadChart() {

    var startPeriod = document.getElementById('startPeriod'); 
    var endPeriod = document.getElementById('endPeriod');
    var currency = document.getElementById('currency');
    var entityId = document.getElementById('entityId');
    var entityType = document.getElementById('entityType');
    var chartElement = document.getElementById('pricesChart');

    var currName = "BYN";

    if (startPeriod === null || endPeriod === null || entityId === null || entityType === null) {
        return;
    }

    //console.log(entityId.value);
    //console.log(endPeriod.value);
    //console.log(startPeriod.value);
    //console.log(entityType.value);

    if (startPeriod.value === "" || endPeriod.value === "" || entityId.value === "" || entityType.value === "") {
        return;
    }    

    let url = `/${entityType.value}/PricesData?id=${entityId.value}&startPeriod=${startPeriod.value}&endPeriod=${endPeriod.value}`;

    
    if (currency != undefined && currency.value != "") {
        url = url + `&currencyId=${currency.value}`;        
    }

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
        currName = chartData[i].currency;
    }

    if (currName === null) currName = "BYN";

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

    if (dataArray.length == 0) {
        return;
    }

    data.addRows(dataArray);

    var options = {
        chart: {
            title: 'Price dynamics',
            subtitle: `in ${currName}`
        },
        width: 900,
        height: 500,
        hAxis: {
            format: 'dd.MM.yyyy'
        },
        vAxis: {
            format: '####.##',
            title: `${currName}`
        }
    };

    var formatter = new google.visualization.NumberFormat(
        { prefix: `${currName} `, negativeColor: 'red', negativeParens: true });

    for (var i = 0; i <= chartData.length; i++) {
        formatter.format(data, i);
    }
    

    var chart = new google.charts.Line(chartElement);

    chart.draw(data, google.charts.Line.convertOptions(options));
}