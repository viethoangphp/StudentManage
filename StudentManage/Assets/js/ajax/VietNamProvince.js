// #region JS Promises region Viet Nam Provinces
const getAllCities = function () {
    return new Promise(function (resolve) {
        $.ajax({
            url: "/Union/GetAllCities",
            method: "GET",
            dataType: "json",
            success: function (data) {
                resolve(data);
            }
        })
    })
}
const getDistrictsByCityID = (cityID) => {
    return new Promise(function (resolve) {
        $.ajax({
            url: "/Union/GetDistrictsByCityID/",
            method: "GET",
            dataType: "json",
            data: { cityID: cityID },
            success: function (listDistrict) {
                resolve(listDistrict);
            }
        })
    })
}
const getWardsByDistrictID = (districtID) => {
    return new Promise(function (resolve) {
        $.ajax({
            url: "/Union/GetWardsByDistrictID/",
            method: "GET",
            dataType: "json",
            data: { districtID: districtID },
            success: function (listWard) {
                resolve(listWard);
            }
        })
    })
}
// #endregion
// #region JS select region loading Viet Nam Provinces
getAllCities().then(function (data) {
    data.forEach(item => {
        city.options[city.options.length] = new Option(item.Name, item.Code)
    });
})
city.onchange = function () {
    district.length = 1
    ward.length = 1
    if (this.value != 0) {
        getDistrictsByCityID(this.value).then(listDistrict => {
            listDistrict.forEach(item => {
                district.options[district.options.length] = new Option(item.Name, item.Code)
            })
        })
    }
}
district.onchange = function () {
    ward.length = 1
    if (this.value != 0) {
        getWardsByDistrictID(this.value).then(listWard => {
            listWard.forEach(item => {
                ward.options[ward.options.length] = new Option(item.Name, item.Code)
            })
        })
    }
}
// #endregion