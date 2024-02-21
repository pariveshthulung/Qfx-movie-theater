$(document).ready(function () {
    loadDataTable();
  });
  
  function loadDataTable() {
    dataTable = $("#myDataTable").DataTable({
      "ajax": { url: '/Admin/OrderManagement/GetAll'},
      "columns": [
      { data: 'user.Name' , "width": "15%" },
      { data: 'user.Email' , "width": "15%" },
      { data: 'user.PhoneNo' , "width": "15%" },
      { data: 'user.DateOfBirth', "width": "15%" },
      { data: 'user.UserStatus' , "width": "15%" },
      { data: 'user.UserType' , "width": "15%"},
      { data: 'user.Location' , "width": "15%"},
      { data: 'user.CreatedDate' , "width": "15%"},
      { data: 'action' , "width": "15%"}
  ]});
  }
  