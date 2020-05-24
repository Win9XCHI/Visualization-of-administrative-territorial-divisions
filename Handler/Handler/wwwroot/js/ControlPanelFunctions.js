
$('#s_alert').hide();
$('#e_alert').hide();

function Download() {
    $('#myModal').modal('show');
}

function Show(data) {
    $('#download').hide();

    if (data == "true") {

        $('#s_alert').show();
        setTimeout(function () {
            let form = document.createElement('form');
            form.action = 'SoursePanel/Index';
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