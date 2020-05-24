
$('#s_alert').hide();
$('#e_alert').hide();

function Download() {
    $('#myModal').modal('show');
}

function Show(data) {
    $('#download').hide();

    if (data != "") {

        $('#s_alert').show();
        setTimeout(function () {
            let form = document.createElement('form');
            form.action = data;
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