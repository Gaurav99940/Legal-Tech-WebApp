function InitiativeDetailSelf(id) {
    $('#initiativeid').text('0');
    var settings = {
        "url": "/API/GetInitiativeDetailById?id=" + id,
        "method": "POST",
        "timeout": 0,
    };
    $.ajax(settings).done(function (response) {
        if (response.initiativestatus == "1" && response.showstatus == "1") {
            var v = document.getElementById('approvebtn');
            if (v != null) {
                document.getElementById('approvebtn').style.display = 'block';
                document.getElementById('rejectbtn').style.display = 'block';
            }
        }
        else {
            var v = document.getElementById('approvebtn');
            if (v != null) {
                document.getElementById('approvebtn').style.display = 'none';
                document.getElementById('rejectbtn').style.display = 'none';
            }
        }
        $('#initiativename').text(response.name);
        $('#descriptionself').text(response.description);
        $('#deliverablesself').text(response.deliverables);
        $('#valueadditiontocompanyself').text(response.valueadditiontocompany);
        $('#initiativeid').text(id);
        var teammember = "";
        response.members.forEach(function (member) {
            teammember += member.name + '; ';
        });
        $('#teammembers').text(teammember);
        var activitieshtml = "";
        response.activities.forEach(function (activitie) {
            activitieshtml += '<tr>';
            activitieshtml += '<td>' + activitie.activityby + '</td>';
            activitieshtml += '<td>' + activitie.activitydate.slice(0, 10) + '</td>';
            activitieshtml += '<td>' + activitie.who + '</td>';
            activitieshtml += '<td>' + activitie.effort + '</td>';
            activitieshtml += '<td>' + activitie.remark + '</td>';
            activitieshtml += '<td><span class="badge ' + activitie.statuscolor + '">' + activitie.statusname + '</span></td>';
            activitieshtml += '</tr>';
        });
        document.getElementById('tractivities').innerHTML = activitieshtml;
    });

    $('#large').modal('show');
}

function InitiativeDetailApproval(id) {
    $('#initiativeid').text('0');
    var settings = {
        "url": "/API/GetInitiativeDetailById?id=" + id,
        "method": "POST",
        "timeout": 0,
    };
    $.ajax(settings).done(function (response) {
        if (response.initiativestatus == "1" && response.showstatus == "1") {
            var v = document.getElementById('approvebtn');
            if (v != null) {
                document.getElementById('approvebtn').style.display = 'block';
                document.getElementById('rejectbtn').style.display = 'block';
            }
        }
        else {
            var v = document.getElementById('approvebtn');
            if (v != null) {
                document.getElementById('approvebtn').style.display = 'none';
                document.getElementById('rejectbtn').style.display = 'none';
            }
        }
        $('#initiativename').text(response.name);
        $('#description').text(response.description);
        $('#deliverables').text(response.deliverables);
        $('#valueadditiontocompany').text(response.valueadditiontocompany);
        $('#initiativeid').text(id);
        if (response.plannedstartdate != "0001-01-01") {
            $('#plannedstartdate').val(response.plannedstartdate);
        }
        if (response.expectedenddate != "0001-01-01") {
            $('#expectedenddate').val(response.expectedenddate);
        }
        var teammember = "";
        response.members.forEach(function (member) {
            teammember += '<div class="col-md-4"><label class="control-label" style="margin-top:1em">' + member.name + '</label></div><div class="col-md-2""><input class="form-control" id="allocation_' + member.id + '" value="' + member.allocation +'" type="number" ></div>';
        });
        $('#teammembers').html(teammember);
        var activitieshtml = "";
        response.activities.forEach(function (activitie) {
            activitieshtml += '<tr>';
            activitieshtml += '<td>' + activitie.activityby + '</td>';
            activitieshtml += '<td>' + activitie.activitydate.slice(0, 10) + '</td>';
            activitieshtml += '<td>' + activitie.who + '</td>';
            activitieshtml += '<td>' + activitie.effort + '</td>';
            activitieshtml += '<td>' + activitie.remark + '</td>';
            activitieshtml += '<td><span class="badge ' + activitie.statuscolor + '">' + activitie.statusname + '</span></td>';
            activitieshtml += '</tr>';
        });
        document.getElementById('tractivities').innerHTML = activitieshtml;
    });

    $('#large').modal('show');
}

function InitiativeDetail(id) {
    $('#initiativeid').text('0');
    var settings = {
        "url": "/API/GetInitiativeDetailById?id=" + id,
        "method": "POST",
        "timeout": 0,
    };
    $.ajax(settings).done(function (response) {
        if (response.initiativestatus == "1" && response.showstatus == "1") {
            var v = document.getElementById('approvebtn');
            if (v != null) {
                document.getElementById('approvebtn').style.display = 'block';
                document.getElementById('rejectbtn').style.display = 'block';
            }
        }
        else {
            var v = document.getElementById('approvebtn');
            if (v != null) {
                document.getElementById('approvebtn').style.display = 'none';
                document.getElementById('rejectbtn').style.display = 'none';
            }
        }
        $('#initiativename').text(response.name);
        $('#description').text(response.description);
        $('#deliverables').text(response.deliverables);
        $('#valueadditiontocompany').text(response.valueadditiontocompany);
        $('#initiativeid').text(id);
        var teammember = "";
        response.members.forEach(function (member) {
            teammember += member.name + '; ';
        });
        $('#teammembers').text(teammember);
        var activitieshtml = "";
        response.activities.forEach(function (activitie) {
            activitieshtml += '<tr>';
            activitieshtml += '<td>' + activitie.activityby + '</td>';
            activitieshtml += '<td>' + activitie.activitydate.slice(0, 10) + '</td>';
            activitieshtml += '<td>' + activitie.who + '</td>';
            activitieshtml += '<td>' + activitie.effort + '</td>';
            activitieshtml += '<td>' + activitie.remark + '</td>';
            activitieshtml += '<td><span class="badge ' + activitie.statuscolor + '">' + activitie.statusname + '</span></td>';
            activitieshtml += '</tr>';
        });
        document.getElementById('tractivities').innerHTML = activitieshtml;
    });

    $('#large').modal('show');
}

function ClearPreviousData() {
    var messageElement = document.getElementById('initiativemessage');
    messageElement.classList.remove('alert');
    messageElement.classList.remove('alert-warning');
    messageElement.classList.remove('alert-success');
    messageElement.innerText = "";
}

function ApproveInitiative() {
    var values = [];
    

    var messageElement = document.getElementById('initiativemessage');
    var form = document.getElementById('large');
    if (form) {
        var inputElements = form.getElementsByTagName('input');
        for (var i = 0; i < inputElements.length; i++) {
            if (inputElements[i].id && inputElements[i].id.startsWith('allocation_')) {
                var row = {};
                if (document.getElementById(inputElements[i].id).value == "") {
                    messageElement.classList.add('alert');
                    messageElement.classList.add('alert-warning');
                    messageElement.innerText = "Please enter resource allocation percentage.";
                    document.getElementById(inputElements[i].id).focus();
                    return;
                }
                else {
                    row["id"] = inputElements[i].id;
                    row["allocation"] = document.getElementById(inputElements[i].id).value;
                }
                values.push(row);
            }
            
        }
    }

    var id = $('#initiativeid').text();
    ClearPreviousData();
    
    var remarks = document.getElementById('initiativeremarks').value.trim();
    var plannedstartdate = document.getElementById('plannedstartdate').value.trim();
    var expectedenddate = document.getElementById('expectedenddate').value.trim();

    if (plannedstartdate == "") {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please enter planned start date.";
    }
    else if (expectedenddate == "") {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please enter expected end date.";
    }
    else if (plannedstartdate > expectedenddate) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Planned started date should be less or equal to expected completion date.";
    }
    else if (remarks == "") {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please enter remarks.";
    }
    else {

        var dataObject = {
            "id": id,
            "remarks": remarks,
            "plannedstartdate": plannedstartdate,
            "expectedenddate": expectedenddate,
            "teamAllocation": values
        };

        var settings = {
            "url": "/API/ApproveInitiative",
            "method": "POST",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(dataObject),
            "timeout": 0,
        };
        $.ajax(settings).done(function (response) {
            InitiativeDetailApprovalClear(id);
            messageElement.classList.add('alert');
            messageElement.classList.add('alert-success');
            messageElement.innerText = response.message;
            document.getElementById('initiativeremarks').value = "";
        });
    }
}

function RejectInitiative() {
    var id = $('#initiativeid').text();
    ClearPreviousData();
    var messageElement = document.getElementById('initiativemessage');
    var remarks = document.getElementById('initiativeremarks').value.trim();
    if (remarks == "") {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please enter remarks.";
    }
    else {
        var settings = {
            "url": "/API/RejectInitiative?id=" + id + "&remarks=" + remarks,
            "method": "POST",
            "timeout": 0,
        };
        $.ajax(settings).done(function (response) {
            messageElement.classList.add('alert');
            messageElement.classList.add('alert-success');
            messageElement.innerText = response.message;
            document.getElementById('initiativeremarks').value = "";
            InitiativeDetail(id);
        });
    }
}

function InitiativeDetailWithClear(id)
{
    ClearPreviousData();
    InitiativeDetail(id);
}

function InitiativeDetailApprovalClear(id) {
    ClearPreviousData();
    InitiativeDetailApproval(id);
}

function CloseModel() {
    window.location.href = "Approval";
}

function CloseModelPopup(page) {
    window.location.href = page;
}

function ClosePresentedModel() {
    window.location.href = "Presented";
}

function CloseCompletedModel() {
    window.location.href = "Completed";
}

function CloseYourRelativeInitiative() {
    window.location.href = "SubmitLog";
} 

function BindInitiativeStatus(statuscontrol, initativeid) {
    const dropdown = document.getElementById(statuscontrol);
    dropdown.innerHTML = '';
    var settings = {
        "url": "/API/GetStatusByInitativeId?initativeid=" + initativeid,
        "method": "POST",
        "timeout": 0,
    };
    $.ajax(settings).done(function (response) {
        const selectoption = document.createElement('option');
        selectoption.value = "0";
        selectoption.text = "Select Status";
        dropdown.appendChild(selectoption);
        response.forEach(function (item) {
            const option = document.createElement('option');
            option.value = item.id; 
            option.text = item.name;
            dropdown.appendChild(option);
            dropdown.value = "4";
        });
    });
}

function BindMemberByInitativeId(membercontrol, initativeid) {
    const dropdown = document.getElementById(membercontrol);
    dropdown.innerHTML = '';
    var settings = {
        "url": "/API/GetMemberByInitativeId?initativeid=" + initativeid,
        "method": "POST",
        "timeout": 0,
    };
    $.ajax(settings).done(function (response) {
        const selectoption = document.createElement('option');
        selectoption.value = "0";
        selectoption.text = "Select Member";
        dropdown.appendChild(selectoption);
        response.forEach(function (item) {
            const option = document.createElement('option');
            option.value = item.id;
            option.text = item.name;
            dropdown.appendChild(option);
        });
    });
}

function SubmitInitiativeLog() {
    ClearPreviousData();
    var id = $('#initiativeid').text();
    var initiativestatus = $('#initiativestatus').val();
    var initiativemember = $('#initiativemembers').val();
    var effort = $('#effort').val();
    var initiativeremarks = $('#initiativeremarks').val().trim();
    if (id == "0") {
        SetMessageErrororWarning("warning", "Invalid Initiative");
    }
    else if (initiativestatus == "0" || initiativestatus == "") {
        SetMessageErrororWarning("warning", "Please select status");
    }
    else if (initiativemember == "0" || initiativemember == "") {
        SetMessageErrororWarning("warning", "Please select who work");
    }
    else if (effort == "0" || effort == "") {
        SetMessageErrororWarning("warning", "Please enter effort hour");
    }
    else if (parseFloat(effort) <= 0) {
        SetMessageErrororWarning("warning", "Please enter effort hour in positive");
    }
    else if (initiativeremarks == "") {
        SetMessageErrororWarning("warning", "Please enter remarks");
    }
    else {
        var settings = {
            "url": "/API/SubmitInitiativeLog?initativeid=" + id + "&memberid=" + initiativemember + "&status=" + initiativestatus + "&remark=" + initiativeremarks + "&effort=" + effort,
            "method": "POST",
            "timeout": 0,
        };
        $.ajax(settings).done(function (response) {
            if (response.code == "0") {
                InitiativeDetail(id);
                BindInitiativeStatus('initiativestatus', id);
                BindMemberByInitativeId('initiativemembers', id);
                document.getElementById('initiativeremarks').value = "";
                document.getElementById('effort').value = "";
                SetMessageErrororWarning("success", response.message);
            }
            else {
                SetMessageErrororWarning("error", response.message);
            }
        });
    }
}

function SetMessageErrororWarning(messagetype, message) {
    var messageElement = document.getElementById('initiativemessage');
    if (messagetype == "warning") {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = message;
    }
    else if (messagetype == "error") {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-danger');
        messageElement.innerText = message;
    }
    else {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-success');
        messageElement.innerText = message;
    }
}

function MarkasCompletedInitiative() {
    var messageElement = document.getElementById('initiativemessage');
    var id = $('#initiativeid').text();
    ClearPreviousData();
    var settings = {
        "url": "/API/MarkasCompletedInitiative?id=" + id,
        "method": "POST",
        "timeout": 0,
    };
    $.ajax(settings).done(function (response) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-success');
        messageElement.innerText = response.message;
        InitiativeDetail(id);
    });
}

function ClearNewLogInitiativeControl() {
    $('#description').text("");
    $('#deliverables').text("");
    $('#valueadditiontocompany').text("");
}

function MarkasPresentedInitiative() {
    var messageElement = document.getElementById('initiativemessage');
    var id = $('#initiativeid').text();
    ClearPreviousData();
    var settings = {
        "url": "/API/MarkasPresentedInitiative?id=" + id,
        "method": "POST",
        "timeout": 0,
    };
    $.ajax(settings).done(function (response) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-success');
        messageElement.innerText = response.message;
        InitiativeDetail(id);
    });
}

function MarkasInProgressInitiative() {
    var messageElement = document.getElementById('initiativemessage');
    var id = $('#initiativeid').text();
    ClearPreviousData();
    var settings = {
        "url": "/API/MarkasInProgressInitiative?id=" + id,
        "method": "POST",
        "timeout": 0,
    };
    $.ajax(settings).done(function (response) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-success');
        messageElement.innerText = response.message;
        InitiativeDetail(id);
    });
}

function MarkasApprovedInitiative() {
    var messageElement = document.getElementById('initiativemessage');
    var id = $('#initiativeid').text();
    ClearPreviousData();
    var settings = {
        "url": "/API/MarkasApprovedInitiative?id=" + id,
        "method": "POST",
        "timeout": 0,
    };
    $.ajax(settings).done(function (response) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-success');
        messageElement.innerText = response.message;
        InitiativeDetail(id);
    });
}

function MarkasInitiated() {
    var messageElement = document.getElementById('initiativemessage');
    var id = $('#initiativeid').text();
    ClearPreviousData();
    var settings = {
        "url": "/API/MarkasInitiated?id=" + id,
        "method": "POST",
        "timeout": 0,
    };
    $.ajax(settings).done(function (response) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-success');
        messageElement.innerText = response.message;
        InitiativeDetail(id);
    });
}


