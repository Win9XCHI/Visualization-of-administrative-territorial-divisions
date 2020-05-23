$("#Log").on("click", function () {
    //$('#download2').hide();
    $('#myModal').modal('show');
    //$('#download').hide();
    //$('#download2').show();


    /*let Form = {
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
    });*/
});

function Download() {
    $('#myModal').modal('show');
}

function Show(data) {
    $('#download').hide();

    if (data == "true") {

        checkmark.classList.add(className);

        setTimeout(function () {
            checkmark.classList.remove(className);
        }, 1700);

    } else {

    }

    setTimeout(sayHi, 1000);
}

function sayHi() {
    alert('Привет');
}

