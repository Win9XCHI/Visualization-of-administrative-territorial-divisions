/*$("#formControlRange").on("slide", function (slideEvt) {
    $("#ex6SliderVal").text(slideEvt.value);
});*/

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
            triangleCoords.push({ lat: parseFloat(data[n - 1].Lat.replace(',', '.')), lng: parseFloat(data[n - 1].Long.replace(',', '.')) });
            GM.AddGF(GF);
            GM.SetObject(NewColor(), triangleCoords);
            return n - 1;
        }

        triangleCoords.push({ lat: parseFloat(data[n].Lat.replace(',', '.')), lng: parseFloat(data[n].Long.replace(',', '.')) });
    }

    GM.AddGF(GF);
    GM.SetObject(NewColor(), triangleCoords);

    return data.length - 1;
}

function RandomColor() {
    let letters = '0123456789ABCDEF';
    color = '#';
    for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }

    return color;
}

function NewColor() {
    
    let color = RandomColor();

    while (GM.GetPalette().indexOf(color) != -1) {

            color = RandomColor();
    }

    GM.AddColor(color);

    return color;
}