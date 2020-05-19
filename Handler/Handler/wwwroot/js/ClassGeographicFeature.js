var GeographicFeature = function (object) {
    this.Name = object.Name;
    this.Information = object.Information;
};

GeographicFeature.prototype.SetTriangle = function (T) {
    this.Triangle = T;
};