function checkLoginConflict() {
    var settings = {
        "url": "/API/CheckLoginConflict",
        "method": "Get",
        "timeout": 0,
    };
    $.ajax(settings).done(function (response) {
        if (response.status == false) {
            window.location.href = '/LoginAgain';

        }
    });
}
setInterval(checkLoginConflict, 5000);

