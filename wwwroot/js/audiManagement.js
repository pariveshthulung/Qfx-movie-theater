$(document).ready(function () {
    loadDataTable1();
  });
          function loadDataTable1() {
    dataTable = $("#audiTable").DataTable({
      "columns": [
      { data: 'audi.Location.CityName' , "width": "15%" },
      { data: 'audi.Name' , "width": "15%" },
      { data: 'audi.Row audi.Column' , "width": "15%" },
  ]});
  }