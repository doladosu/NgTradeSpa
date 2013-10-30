﻿AmCharts.ready(function () {
  generateChartData();
  createStockChart();
});

var chartData = [];

function generateChartData() {
  var firstDate = new Date(2012, 0, 1);
  firstDate.setDate(firstDate.getDate() - 500);
  firstDate.setHours(0, 0, 0, 0);

  for (var i = 0; i < 500; i++) {
    var newDate = new Date(firstDate);
    newDate.setDate(newDate.getDate() + i);

    var value = Math.round(Math.random() * (40 + i)) + 100 + i;

    chartData.push({
      date: newDate,
      value: value
    });
  }
}


function createStockChart() {
  var chart = new AmCharts.AmStockChart();
  chart.pathToImages = "../amcharts/images/";

  // DATASETS //////////////////////////////////////////
  var dataSet = new AmCharts.DataSet();
  dataSet.color = "#b0de09";
  dataSet.fieldMappings = [{
    fromField: "value",
    toField: "value"
  }];
  dataSet.dataProvider = chartData;
  dataSet.categoryField = "date";

  chart.dataSets = [dataSet];

  // PANELS ///////////////////////////////////////////                                                  
  var stockPanel = new AmCharts.StockPanel();
  stockPanel.showCategoryAxis = true;
  stockPanel.title = "Value";
  stockPanel.eraseAll = false;
  stockPanel.addLabel(0, 100, "Click on the pencil icon on top-right to start drawing", "center", 16);

  var graph = new AmCharts.StockGraph();
  graph.valueField = "value";
  graph.bullet = "round";
  graph.bulletColor = "#FFFFFF";
  graph.bulletBorderColor = "#00BBCC";
  graph.bulletBorderAlpha = 1;
  graph.bulletBorderThickness = 2;
  graph.bulletSize = 7;
  graph.lineThickness = 2;
  graph.lineColor = "#00BBCC";
  graph.useDataSetColors = false;
  stockPanel.addStockGraph(graph);

  var stockLegend = new AmCharts.StockLegend();
  stockLegend.valueTextRegular = " ";
  stockLegend.markerType = "none";
  stockPanel.stockLegend = stockLegend;
  stockPanel.drawingIconsEnabled = true;

  chart.panels = [stockPanel];


  // OTHER SETTINGS ////////////////////////////////////
  var scrollbarSettings = new AmCharts.ChartScrollbarSettings();
  scrollbarSettings.graph = graph;
  scrollbarSettings.updateOnReleaseOnly = true;
  chart.chartScrollbarSettings = scrollbarSettings;

  var cursorSettings = new AmCharts.ChartCursorSettings();
  cursorSettings.valueBalloonsEnabled = true;
  chart.chartCursorSettings = cursorSettings;


  // PERIOD SELECTOR ///////////////////////////////////
  var periodSelector = new AmCharts.PeriodSelector();
  periodSelector.position = "bottom";
  periodSelector.periods = [{
    period: "DD",
    count: 10,
    label: "10 days"
  }, {
    period: "MM",
    count: 1,
    label: "1 month"
  }, {
    period: "YYYY",
    count: 1,
    label: "1 year"
  }, {
    period: "YTD",
    label: "YTD"
  }, {
    period: "MAX",
    label: "MAX"
  }];
  chart.periodSelector = periodSelector;

  var panelsSettings = new AmCharts.PanelsSettings();
  chart.panelsSettings = panelsSettings;

  chart.write('chartdiv');
}