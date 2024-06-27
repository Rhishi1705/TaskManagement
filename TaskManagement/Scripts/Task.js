


$(document).ready(function () {

});

function add() {
    $(".content-wrapper > h3").addClass("open");
    $("#hdn_id").val(0);
    $(".grid-view,.metadata").addClass("hidden");
    $(".form-view").removeClass("hidden");
    $(".actions").find(".delete,.edit").addClass("hidden");
    $(".actions").find(".add").removeClass("hidden");

}
function closeform() {
    window.location.href = window.location.href;
}
function binddata(opt, el, id) {
    if (opt == "fill") {
        $("#hdn_id").val(id);
        $(".content-wrapper > h3").addClass("open");
        $(".grid-view").addClass("hidden");
        $(".form-view,.metadata").removeClass("hidden");
        el = $(el).parent();
        $('#cTitle').val($.trim($(el).find("td").eq(1).text()));
        $('#cDecription').val($.trim($(el).find("td").eq(2).text()));
        $('#statusddl').val($.trim($(el).find("td").eq(5).text()));

        let dateString = ($.trim($(el).find("td").eq(3).text()));
        let parts = dateString.split('/');
        let formattedDate = `${parts[2]}-${parts[1]}-${parts[0]}`;
        document.getElementById('dDueDate').value = formattedDate;

        $('.form-view').find("input[type=\"text\"],input[type=\"date\"],select").prop("disabled", true);
        $(".actions").find(".delete,.edit").removeClass("hidden");
        $(".actions").find(".save").addClass("hidden");

        var dCreatedDate = ($.trim($(el).find("td").eq(7).text()));
        var dModifiedDate = ($.trim($(el).find("td").eq(8).text()));
        var cCreatedBy = ($.trim($(el).find("td").eq(9).text()));
        var cModifiedBy = ($.trim($(el).find("td").eq(10).text()));
        var metadataElement = document.getElementById('metadata');
        metadataElement.innerHTML = ' <span>Created by: ' + cCreatedBy + ' (' + dCreatedDate + ') </span> <span>Last Modified by: ' + cModifiedBy + ' (' + dModifiedDate + ')</span>';
    
    }
    else if (opt == "edit") {        
        $(".actions").find(".delete,.edit").addClass("hidden");
        $(".actions").find(".save").removeClass("hidden");
        $('.form-view').find("input[type=\"text\"],input[type=\"date\"],select").prop("disabled", false);
    }
}

function savedata() {

    var cTitle = $.trim($("#cTitle").val());
    if (cTitle == "" || cTitle == null || cTitle == undefined) {
        alertify.error("Please Enter Title");
        $("#cTitle").focus();
        return false;
    }
    var cDecription = $.trim($("#cDecription").val());
    if (cDecription == "" || cDecription == null || cDecription == undefined) {
        alertify.error("Please Enter Description");
        $("#cDecription").focus();
        return false;
    }

    var dDueDate=document.getElementById('dDueDate').value;
    if(dDueDate==""||dDueDate==null||dDueDate==undefined)
    {
        alertify.error("Please Select Due Date");
        $("#dDueDate").focus();
        return false;
    }
   
    var nStatus = $("#statusddl").val();
    if (nStatus == 0 || nStatus == null) {
        alertify.error("Please Select Status");
        $("#statusddl").focus();
        return false;
    }

    BindDataModel();
}

function BindDataModel() {

    var Model = {
        nTaskId: $("#hdn_id").val(),
        cTitle: $("#cTitle").val(),
        cDecription: $("#cDecription").val(),
        dDueDate: document.getElementById('dDueDate').value,
        nStatus: $("#statusddl").val(),
    };

    $.ajax({
        type: 'POST',
        url: "/Task/SaveData",
        data: JSON.stringify(Model),
        async: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {            
            if (data > 0) {
                if ($("#hdn_id").val() == 0) {
                    alertify.success("Task Created Successfully");
                }
                else {
                    alertify.success("Task Updated Successfully");
                }
                window.location.href = window.location.href;
            }
            else {
                alertify.error("Some error has occured");
            }
        }
    });
}

function deletedata(id) {
    if (!id) {
        id = $("#hdn_id").val();
    }
    var userConfirmed = confirm("Are you sure you want to delete ?");
    if (userConfirmed) {
        var Model = {};
        Model.nTaskId = id;
        $.ajax({
            type: 'POST',
            url: "/Task/Delete",
            data: JSON.stringify(Model),
            async: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                if (data > 0) {
                    alertify.success("Task Deleted Successsfully");
                    window.location.href = window.location.href;
                }
                else {
                    alertify.error("Some error has occured");
                }
            }
        });
    }
}


function FilterChange(dropdown) {
    var input, table, tr, td, i, j, txtValue;
    var SelectedText="";
    var selectedValue = dropdown.value;
    if(selectedValue==1)
    {
        SelectedText="InCompleted";
    }
    else if(selectedValue==2)
    {
        SelectedText="Completed";
    }
    input = SelectedText;
    table = document.getElementById('TaskTbl');
    tr = table.getElementsByTagName('tr');

    for (i = 1; i < tr.length; i++) {
        tr[i].style.display = 'none'; // Hide the row initially
        td = tr[i].getElementsByTagName('td');

        for (j = 0; j < td.length; j++) {
            if (td[j]) {
                txtValue = td[j].textContent || td[j].innerText;
                if (txtValue.trim()==input.trim()) {
                    tr[i].style.display = ''; // Show the row if a match is found
                    break; 
                }
            }
        }
    }
}