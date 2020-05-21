function showMarker(event) {
    let stringContent;

    for (let i = 0; i < GM.ArrayGeographicFeature.length; i++) {
        if (this === GM.ArrayGeographicFeature[i].GetTriangle()) {
            stringContent = '<div>' +
                '<p class="text-center">' + GM.ArrayGeographicFeature[i].GetName() + '</p>' +
                '<p class="text-center">' + GM.ArrayGeographicFeature[i].GetInformation() + '</p>' +
                '<a href="Search/DetailsInfo?name=' + GM.ArrayGeographicFeature[i].GetName() + '">Детально</a>' +
                '</div>';
            break;
        }
    }

    this.infoWindow = new google.maps.InfoWindow({
        content: stringContent,
        position: event.latLng
    });

    this.infoWindow.open(this.map);
};


$("#Output").on("click", function () {
    GM.Clear();

    if (isNaN($("#InputYear1").val())) {
        return;
    }

    let FormMap = {
        Year: $("#InputYear1").val(),
        Level: $("#formControlRange").val(),
        Exeptions: $("#InputExeptions1").val()
    }

    $.ajax({
        url: "Visualization/MapView",
        type: "POST",
        dataType: "json",
        cache: false,
        data: ({
            map: FormMap
        }),
        beforeSend: Download,
        success: Show
    });
});