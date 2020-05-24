
$("#Submit").on("click", function () {

    let FormRegister = {
        PIB: $("#validationCustom01").val(),
        Phone: $("#validationCustom02").val(),
        Birthday: $("#validationCustom04").val(),
        Login: $("#validationCustomLogin").val(),
        Password: $("#validationCustom03").val(),
        DiplomaF: $("#customFile").val()
    }

    if (Validation(FormRegister)) {

        /*$.ajax({
            url: "Authentication/NewUser",
            type: "POST",
            dataType: "json",
            cache: false,
            data: ({
                User: FormRegister
            }),
            beforeSend: Download
        });*/
    } 
});

function Validation(FormRegister) {

    /*if (FormRegister.PIB.split(' ').length != 3 || FormRegister.PIB.search('/\d/') != -1) {

    }*/


    return true;
}

function Download() {
    $('#myModal').modal('show');
}
