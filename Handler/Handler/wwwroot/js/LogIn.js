
$("#Log").on("click", function () {

    let Form = {
        Code: 0,
        PIB: "",
        Rights: "",
        Login: $("#inputLogin").val(),
        Password: $("#inputPassword").val()
    }

    $.ajax({ 
        url: "Authentication/Check",
        type: "POST",
        dataType: "json",
        cache: false,
        data: ({
            User: Form
        }),
        beforeSend: Download,
        success: Show
    });
});

function Download() {
    $('#myModal').modal('show');
}

function Show(data) {
    $('#download').hide();

    if (data == "true") {

        $('#s_alert').show();
        setTimeout(function () {
            let form = document.createElement('form');
            form.action = 'Authentication/Redirect';
            form.method = 'GET';
            form.innerHTML = '';
            document.body.append(form);
            form.submit();
        }, 2000);

    } else {
        $('#e_alert').show();
        setTimeout(function () {
            $('#e_alert').hide();
            $('#myModal').modal('hide');
            $('#download').show();
        }, 2000);
    }
}

function Route() {
    $.ajax({
        url: "Authentication/Redirect",
        type: "GET",
        dataType: "json",
        cache: false
    });
}

$('#s_alert').hide();
$('#e_alert').hide();