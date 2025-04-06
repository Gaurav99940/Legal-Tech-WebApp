function StatusUpdate(status, id) {
    var v = 0;
    var vControl = document.getElementById("status_" + id);
    if (vControl.checked == 1) {
        v = 1;
    }
    else {
        v = 0;
    }
    var settings = {
        "url": "/API/UpdateUserStatus?status=" + v + "&uid=" + id,
        "method": "POST",
        "dataType": "json",
        "timeout": 0,
    };
    $.ajax(settings).done(function (response) {
    });
}