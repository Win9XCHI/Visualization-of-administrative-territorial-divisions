var GM;
function initMap() {
    GM = new GoogleMap();
    GM.SetMap(new google.maps.Map(document.getElementById("map"), GM.GetOptions()));
}

$("#formControlRange").on("slide", function (slideEvt) {
    $("#ex6SliderVal").text(slideEvt.value);
});

$("#Output").on("click", function () {

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

function Download() {

    $('#myModal').modal('show');
}

function Show(data) {

    data = JSON.parse(data);

    for (let i = 0; i < data.length; i++) {
        
        if (data[i].NumberRecord == 1) {

            i = CreateObject(data, i);
        }
    }

    $('#myModal').modal('hide');
}

function CreateObject(data, i) {

    var GF = new GeographicFeature(data[i]);

    var triangleCoords = [];
   
    for (let n = i; n < data.length; n++) {

        if (data[n].NumberRecord == 1 && n != i) {
            return n - 1;
        }

        triangleCoords.push({ lat: data[i].Lat, lng: data[i].Long });
    }

    GM.AddGF(GF);
    GM.SetPoligon(NewColor(), triangleCoords);
}

function NewColor() {
    let letters = '0123456789ABCDEF';
    let color = '#';

    while (Palette.find(function (element) {
        return element === color;
    }).length != 0 && color == '#') {

        color = '#';
        for (let i = 0; i < 6; i++) {
            color += letters[Math.floor(Math.random() * 16)];
        }
    }

    Palette.push(color);

    return color;
}

/*
 * Year: $("#InputYear1").val(),
            Level: $("#formControlRange").val(),
            Exeption: $("#InputExeptions1").val()

 * function initialize() {
           var mapOptions = {
               zoom: 6,
               center: new google.maps.LatLng(48.45, 35.02)
           };

           map = new google.maps.Map(document.getElementById("map"), mapOptions);
       } //Намалювати мапу

       google.maps.event.addDomListener(window, "load", initialize);

       function SetMarker(location, title, MarkerCounter) {

           if (MarkerCounter == 1) {
               marker1 = new google.maps.Marker({
                   position: location, //new google.maps.LatLng(locations[i][1], locations[i][2]),
                   map: map,
                   title: title
               });
           }

           if (MarkerCounter == 2) {
               marker2 = new google.maps.Marker({
                   position: location, //new google.maps.LatLng(locations[i][1], locations[i][2]),
                   map: map,
                   title: title
               });
           }

           if (MarkerCounter == 3) {
               marker3 = new google.maps.Marker({
                   position: location, //new google.maps.LatLng(locations[i][1], locations[i][2]),
                   map: map,
                   title: title
               });
           }
           Mark++;

       } //Поставити маркер

       function SetLine(flightPlanCoordinates) {
           flightPath = new google.maps.Polyline({
               path: flightPlanCoordinates,
               geodesic: true,
               strokeColor: '#FF0000',
               strokeOpacity: 1.0,
               strokeWeight: 2
           });
           flightPath.setMap(map);
           flagLine = true;
       } //Відмалювати криву

       function ClearMarkerAndLine() {
           if (flagLine) {
               marker1.setMap(null);
               marker2.setMap(null);
               if (Mark > 2) {
                   marker3.setMap(null);
               }
               flightPath.setMap(null);
               flagLine = false;
               Mark = 0;
           }
       } //Прибрати маркери та криву
       */