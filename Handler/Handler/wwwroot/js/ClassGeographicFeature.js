var GeographicFeature = function (object) {
    this.Name = object.Name;
    this.Information = object.Information;
};

GeographicFeature.prototype.SetTriangle = function (T) {
    this.Triangle = T;
};

GeographicFeature.prototype.GetTriangle = function () {
    return this.Triangle;
};

GeographicFeature.prototype.GetName = function () {
    return this.Name;
};

GeographicFeature.prototype.GetInformation = function () {
    return this.Information;
};
