
$("#OutputSourse").on("click", function () {
    $('#myModal').modal('show');
});

$("#Save").on("click", function () {
    $('#myModal').modal('show');

    let Form = {
        Name: $("#inputName").val(),
        Author: $("#inputAuthor").val(),
        Year: $("#inputYear").val(),
        YearRelevance: $("#inputYearRelevance").val(),
        Type: $("#inputType").val()
    }

    $.ajax({
        url: "SoursePanel/SaveEdit",
        type: "POST",
        dataType: "json",
        cache: false,
        data: ({
            ObS: Form
        }),
        beforeSend: Download,
        success: Show
    });
});