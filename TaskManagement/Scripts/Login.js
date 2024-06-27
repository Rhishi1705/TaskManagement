


$(document).ready(function () {

});


function savedata() {

    var cUserName = $.trim($("#cUserName").val());
    if (cUserName == "" || cUserName == null || cUserName == undefined) {
        alertify.error("Please Enter UserName");
        $("#cUserName").focus();
        return false;
    }
    var cPassWord = $.trim($("#cPassWord").val());
    if (cPassWord == "" || cPassWord == null || cPassWord == undefined) {
        alertify.error("Please Enter PassWord");
        $("#cPassWord").focus();
        return false;
    }    

    BindDataModel();
}

function BindDataModel() {

    var Model = {
        cUserName: $("#cUserName").val(),
        cPassWord: $("#cPassWord").val(),  
    };

    $.ajax({
        type: 'POST',
        url: "/Login/LoginCheck",
        data: JSON.stringify(Model),
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data > 0) {           
                window.location.href = '/Task/Index';
            }
            else {
                alertify.error("Incorrect UserName Or PassWord, Please Check It.");
            }
        }
    });
}
