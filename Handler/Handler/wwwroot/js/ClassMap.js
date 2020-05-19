var GoogleMap = function () {
    this.mapOptions = {
        zoom: 6,
        center: new google.maps.LatLng(48.45, 35.02),
        mapTypeId: 'satellite'
    };

    this.ArrayGeographicFeature = [];
    this.Palette = [];
    this.Triangles = [];
    this.InfoWindow;
};

GoogleMap.prototype.GetOptions = function () {
    return this.mapOptions;
};

GoogleMap.prototype.SetMap = function (Gm) {
    this.map = Gm;
};

GoogleMap.prototype.SetPoligon = function (color, triangleCoords) {
    let Triangle = new google.maps.Polygon({
        paths: triangleCoords,
        strokeColor: color,
        strokeOpacity: 0.8,
        strokeWeight: 5,
        fillColor: color,
        fillOpacity: 0.35
    });

    Triangle.setMap(this.map);
    this.Triangles.push(Triangle);
    this.ArrayGeographicFeature[this.ArrayGeographicFeature.length - 1].SetTriangle(Triangle);

    Triangle.addListener('click', GoogleMap.showMarker);

    this.infoWindow = new google.maps.InfoWindow;
};

GoogleMap.prototype.showMarker = function (event) {
    var vertices = this.getPath();

    var contentString = '<b>Bermuda Triangle polygon</b><br>' +
        'Clicked location: <br>' + event.latLng.lat() + ',' + event.latLng.lng() +
        '<br>';

    for (var i = 0; i < vertices.getLength(); i++) {
        var xy = vertices.getAt(i);
        contentString += '<br>' + 'Coordinate ' + i + ':<br>' + xy.lat() + ',' +
            xy.lng();
    }

    infoWindow.setContent(contentString);
    infoWindow.setPosition(event.latLng);

    infoWindow.open(map);
};

GoogleMap.prototype.Clear = function () {


};

GoogleMap.prototype.AddGF = function (GF) {
    this.ArrayGeographicFeature.push(GF);
}