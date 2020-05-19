var GoogleMap = function () {
    this.mapOptions = {
        zoom: 6,
        center: new google.maps.LatLng(48.45, 35.02),
        mapTypeId: 'satellite'
    };

    this.ArrayGeographicFeature = [];
    this.Palette = [];
    this.Triangles = [];
    this.Markers = [];
};

GoogleMap.prototype.GetOptions = function () {
    return this.mapOptions;
};

GoogleMap.prototype.SetMap = function (Gm) {
    this.map = Gm;
};

GoogleMap.prototype.SetObject = function (color, triangleCoords) {

    if (triangleCoords.length > 2) {
        this.SetPoligon(color, triangleCoords);

    } else {
        this.SetMarker(color, triangleCoords);
    }
};

GoogleMap.prototype.SetMarker = function (color, triangleCoords) {
    let Marker = new google.maps.Marker({
        position: triangleCoords[0],
        map: map
    });

    Marker.setMap(this.map);
    this.Markers.push(Marker);
    this.ArrayGeographicFeature[this.ArrayGeographicFeature.length - 1].SetTriangle(Marker);
    Marker.addListener('click', showMarker);
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

    Triangle.addListener('click', showMarker);

};

GoogleMap.prototype.Clear = function () {

    this.ArrayGeographicFeature = [];
    this.Palette = [];

    for (let i = 0; i < this.Triangles.length; i++) {
        this.Triangles[i].setMap(null);
    }
    for (let i = 0; i < this.Markers.length; i++) {
        this.Markers[i].setMap(null);
    }

    this.Markers = [];
    this.Triangles = [];
};

GoogleMap.prototype.AddGF = function (GF) {
    this.ArrayGeographicFeature.push(GF);
}

GoogleMap.prototype.GetPalette = function () {
    return this.Palette;
};

GoogleMap.prototype.AddColor = function (color) {
    this.Palette.push(color);
}