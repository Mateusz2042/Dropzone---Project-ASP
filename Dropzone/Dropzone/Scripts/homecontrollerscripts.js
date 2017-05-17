$(function () {
       $("#FileName").autocomplete({  
           source: function(request,response) {  
               $.ajax({  
                   url: "/Home/ShowUploadedFiles",  
                   type: "POST",  
                   dataType: "json",  
                   data: { text: request.term },  
                   success: function (data) {  
                       response($.map(data, function (item) {  
                           return { label: item.FileName, value: item.FileName };  
                       }))  
  
                   }  
               })  
           },  
           messages: {  
               noResults: "", results: ""  
           }  
       });  
})  

function loadFoundFiles() {
    $('#items').load('ShowFoundFiles', {
        text: $('#FileName').val()
    });
}