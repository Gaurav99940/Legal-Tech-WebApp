function BindSites(selectElement) {
    var selectedValue = $(selectElement).val();
    const dropdown = document.getElementById("SiteId");
    dropdown.innerHTML = '';
    const selectoption = document.createElement('option');
    
    dropdown.appendChild(selectoption);
    var settings = {
        "url": "/API/GetSiteList?companyname=" + selectedValue,
        "method": "POST",
        "timeout": 0,
    };
    $.ajax(settings).done(function (response) {
        response.forEach(function (item) {
            const option = document.createElement('option');
            option.value = item.id;
            option.text = item.name;
            dropdown.appendChild(option);
        });
    });
}
function BindCity(selectElement)
{
    BindUser(selectElement);
    BindCampaning(selectElement);
    var selectedValue = $(selectElement).val();
    const dropdown = document.getElementById("CityId");
    dropdown.innerHTML = '';
    const selectoption = document.createElement('option');
    selectoption.value = "0";
    selectoption.text = "Select City";
    dropdown.appendChild(selectoption);
    var settings = {
        "url": "/API/GetCityList?companyname=" + selectedValue,
        "method": "POST",
        "timeout": 0,
    };
    $.ajax(settings).done(function (response) {
        response.forEach(function (item) {
            const option = document.createElement('option');
            option.value = item.id;
            option.text = item.name;
            dropdown.appendChild(option);
        });
    });
}

function BindUser(selectElement) {
    var selectedValue = $(selectElement).val();
    const dropdown = document.getElementById("UserId");
    dropdown.innerHTML = '';
    const selectoption = document.createElement('option');
    //selectoption.value = "0";
    //selectoption.text = "Select User";
    //dropdown.appendChild(selectoption);
    var settings = {
        "url": "/API/GetUserList?companyname=" + selectedValue,
        "method": "POST",
        "timeout": 0,
    };
    $.ajax(settings).done(function (response) {
        response.forEach(function (item) {
            const option = document.createElement('option');
            option.value = item.id;
            option.text = item.name;
            dropdown.appendChild(option);
        });
    });
} 

function BindCampaning(selectElement) {
    var selectedValue = $(selectElement).val();
    const dropdown = document.getElementById("CampaningId");
    dropdown.innerHTML = '';
    const selectoption = document.createElement('option');
    //selectoption.value = "0";
    //selectoption.text = "Select User";
    //dropdown.appendChild(selectoption);
    var settings = {
        "url": "/API/GetCampaningList?companyname=" + selectedValue,
        "method": "POST",
        "timeout": 0,
    };
    $.ajax(settings).done(function (response) {
        response.forEach(function (item) {
            const option = document.createElement('option');
            option.value = item.id;
            option.text = item.name;
            dropdown.appendChild(option);
        });
    });
}

$(document).ready(function () {
    BindCity('databasename');
});

function SaveOutlet() {
    ClearShowMessage();
    var messageElement = document.getElementById('showmessage');

    var databasename = $('#databasename').val();
    var CityId = $('#CityId').val();
    var CityName = $('#CityId option:selected').text();
    var UserIdList = [];
    var CampaningIdList = [];
    var Name = $('#Name').val();
    var Panel = $('#Panel').val();
    var Type = $('#Type').val();
    var Medium = $('#Medium').val();
    var Length = $('#Length').val();
    var Width = $('#Width').val();
    var Address = $('#Address').val();
    var Lat = $('#Lat').val();
    var Long = $('#Long').val();
    var Pincode = $('#Pincode').val();

    var userElement = document.getElementById("UserId"); 
    for (var i = 0; i < userElement.options.length; i++) {
        if (userElement.options[i].selected) {
            UserIdList.push(userElement.options[i].value);
        }
    }
    
    var campaningElement = document.getElementById("CampaningId");
    for (var i = 0; i < campaningElement.options.length; i++) {
        if (campaningElement.options[i].selected) {
            CampaningIdList.push(campaningElement.options[i].value);
        }
    }

    if (databasename == "0" || databasename == "") {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please select company.";
        $('#databasename').focus();
    }
    else if (CityId == "0" || CityId == "") {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please select city.";
        $('#CityId').focus();
    }
    else if (UserIdList.length == 0 || UserIdList == null) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please select user.";
        $('#UserId').focus();
    }
    else if (CampaningIdList.length == 0 || CampaningIdList == null) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please select Campaning.";
        $('#CampaningId').focus();
    }
    else if (Name == "" || Name == null) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please enter Site Name.";
        $('#Name').focus();
    }
    else if (Panel == "" || Panel == null) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please enter Panel.";
        $('#Panel').focus();
    }
    else if (Type == "" || Type == null) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please enter Type.";
        $('#Type').focus();
    }
    else if (Medium == "" || Medium == null) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please enter Medium.";
        $('#Medium').focus();
    }
    else if (Length == "" || Length == null) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please enter Length.";
        $('#Length').focus();
    }
    else if (Width == "" || Width == null) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please enter Width.";
        $('#Width').focus();
    }
    else if (Address == "" || Address == null) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please enter Address.";
        $('#Address').focus();
    }
    else if (Lat == "" || Lat == null) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please enter Latitude.";
        $('#Lat').focus();
    }
    else if (Long == "" || Long == null) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please enter Longitude.";
        $('#Long').focus();
    }
    else if (Pincode == "" || Pincode == null) {
        messageElement.classList.add('alert');
        messageElement.classList.add('alert-warning');
        messageElement.innerText = "Please enter Pincode.";
        $('#Pincode').focus();
    }
    else {
        var dataObject = {
            "CityName": CityName,
            "databasename": databasename,
            "CityId": CityId,
            "UserIdList": UserIdList,
            "CampaningIdList": CampaningIdList,
            "Name": Name,
            "Panel": Panel,
            "Type": Type,
            "Medium": Medium,
            "Length": Length,
            "Width": Width,
            "Address": Address,
            "Lat": Lat,
            "Long": Long,
            "Pincode": Pincode
        };


        var settings = {
            "url": "/API/SaveSiteDetail",
            "method": "POST",
            "contentType": "application/json; charset=utf-8",
            "dataType": "json",
            "data": JSON.stringify(dataObject),
            "timeout": 0,
        };
        $.ajax(settings).done(function (response) {
            if (response.code == "0") {
                messageElement.classList.add('alert');
                messageElement.classList.add('alert-success');
                messageElement.innerText = response.message;
                ClearElements();
            }
            else {
                messageElement.classList.add('alert');
                messageElement.classList.add('alert-error');
                messageElement.innerText = response.message;
            }
            
        });
    }
}

function ClearShowMessage() {
    var messageElement = document.getElementById('showmessage');
    messageElement.classList.remove('alert');
    messageElement.classList.remove('alert-warning');
    messageElement.classList.remove('alert-success');
    messageElement.innerText = "";
}
function ClearElements() {
    $('#databasename').val("");
    BindCity('databasename');
    $('#Name').val("");
    $('#Panel').val("");
    $('#Type').val("");
    $('#Medium').val("");
    $('#Length').val("");
    $('#Width').val("");
    $('#Address').val("");
    $('#Lat').val("");
    $('#Long').val("");
    $('#Pincode').val("");
}

function SetMessageErrororWarning(messagetype, message) {
    var messageElement = document.getElementById('showmessage');
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

function ShowAllSites(id, company,name) {
    document.getElementById('sitelistbyid').innerHTML = "";
    document.getElementById('CampaningNameTag').innerHTML = "";
    document.getElementById('CampaningNameTag').innerHTML = name;
   
    var settings = {
        "url": "/API/GetSiteDetail?id=" + id + "&Company=" + company,
        "method": "POST",
        "timeout": 0,
    };

    $.ajax(settings).done(function (response) {
       
        var sitehtml = "";
        response.forEach(function (sites) {
            sitehtml += '<tr>';
            sitehtml += '<td>' + sites.location + '</td>';
            sitehtml += '<td>' + sites.city + '</td>';
            sitehtml += '<td>' + sites.panel + '</td>';
            sitehtml += '<td>' + sites.type + '</td>';
            sitehtml += '<td>' + sites.medium + '</td>';
            sitehtml += '<td>' + sites.width + '</td>';
            sitehtml += '<td>' + sites.height + '</td>';
            sitehtml += '<td>' + sites.latitude + '</td>';
            sitehtml += '<td>' + sites.longitude + '</td>';
            sitehtml += '</tr>';
        });
        document.getElementById('sitelistbyid').innerHTML = sitehtml;
    });


    $('#large').modal('show');
}