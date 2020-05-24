window.addEventListener("load", function (event) {
    $('#download').hide();
    $('#s_alert').show();
    setTimeout(function () {
        let form = document.createElement('form');
        form.action = 'SoursePanel/Index';
        form.method = 'GET';
        form.innerHTML = '';
        document.body.append(form);
        form.submit();
    }, 2000);
});