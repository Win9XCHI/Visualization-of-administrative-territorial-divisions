function showMarker(event) {
    let stringContent;

    for (let i = 0; i < GM.ArrayGeographicFeature.length; i++) {
        if (this === GM.ArrayGeographicFeature[i].GetTriangle()) {
            stringContent = '<div>' +
                '<p class="text-center">' + GM.ArrayGeographicFeature[i].GetName() + '</p>' +
                '<p class="text-center">' + GM.ArrayGeographicFeature[i].GetInformation() + '</p>' +
                '<a href="https://developers.google.com/maps/documentation/javascript/examples/infowindow-simple" asp-area="" asp-controller="Search" asp-action="Index">Детально</a>' +
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