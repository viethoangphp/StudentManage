$(document).ready(function () {
    //Call ProvinceAPI to fill data to combobox
    //const city = document.getElementById("city_update")
    //const district = document.getElementById("district_update")
    //const ward = document.getElementById("ward_update")
    //var api = "https://provinces.open-api.vn/api/?depth=3";
    //fetch(api, {
    //    method: "GET",
    //    ContentType: "application/json"
    //})
    //    .then(response => response.json())
    //    .then(function (data) {
    //        let count = 0;
    //        data.forEach(item => {
    //            var newOption = new Option(item.name, item.code);
    //            city.append(newOption).trigger('change');
    //            if (cityID != 0) {
    //                if (cityID == city.options[count].value) {
    //                    city.options[count].selected = true
    //                }
    //                count++;
    //            }

    //        });
    //        $('#edit_employee select').trigger("change");
    //        if (cityID != 0) {
    //            count = 0;
    //            var listDistrict = data.filter(m => m.code == cityID)
    //            listDistrict[0].districts.forEach(item => {
    //                district.options[district.options.length] = new Option(item.name, item.code)
    //                if (districtID != 0) {
    //                    if (districtID == district.options[count].value) {
    //                        district.options[count].selected = true
    //                    }
    //                    count++;
    //                }
    //            })
    //        }

    //        if (districtID != 0) {
    //            count = 0;
    //            var listWard = listDistrict[0].districts.filter(m => m.code == districtID)
    //            listWard[0].wards.forEach(item => {
    //                ward.options[ward.options.length] = new Option(item.name, item.code)
    //                if (wardID != 0) {
    //                    if (wardID == ward.options[count].value) {
    //                        ward.options[count].selected = true
    //                    }
    //                    count++;
    //                }
    //            })
    //        }
    //        var listDistrict;
    //        var listWard;
    //        city.onchange = function () {
    //            district.length = 1
    //            ward.length = 1
    //            if (this.value != 0) {
    //                listDistrict = data.filter(m => m.code == this.value)
    //                listDistrict[0].districts.forEach(item => {
    //                    district.options[district.options.length] = new Option(item.name, item.code)
    //                })

    //            }
    //        }
    //        district.onchange = function () {
    //            ward.length = 1
    //            if (this.value != 0) {
    //                var listWard = listDistrict[0].districts.filter(m => m.code == this.value)
    //                listWard[0].wards.forEach(item => {
    //                    ward.options[ward.options.length] = new Option(item.name, item.code)
    //                })
    //            }
    //        }
    //    })
    //    .catch(err => console.log("Lỗi"))
    //Get user info to edit form
    $(".edit-user-action").on("click", function () {
        var id = $(this).data("id");
        getUserInfo(id).then(function (data) {
            $("#user-id").val(data.userID);
            $("#address").val(data.address);
            $("#phone").val(data.phone);
            $("#full-name").val(data.fullname);
            //$("#password").val(data.userID);
            $("#birthday").val(data.birthday);
            $("#email").val(data.email);
            $("#join-date").val(data.joinDate);
            $("#gender").val(data.gender).trigger('change');
            $("#position").val(data.positionID).trigger('change');
            $("#group").val(data.groupID).trigger('change');
            //$("#ward-id").val(data.cityID).trigger('change');
            //$("#district-id").val(data.districtID).trigger('change');
            //$("#city-id").val(data.wardID).trigger('change');
            //$('#position').trigger('change'); // Notify any JS components that the value changed 
            //$('#group').trigger('change'); // Notify any JS components that the value changed
            //$("#status").val(data.status);
        });
    });
    //Reset form when exit
    $(".edit-close").on("click", function () {
        document.getElementById("editUser").reset();
    });
    //Send updated info to server to update user info
    $("#editUser").on("submit", function () {
        var formData = new FormData(this);
        //call update user info here
    });
    //Delete option selected
    $(".delete-user-action").on("click", function () {
        var id = $(this).data("id");
        $(".continue-btn").attr("value",id);
    });
    //Delete confirmed
    //$(".delete-btn").on("click", function () {
    //    var id = $(this).data("id");
    //    deleteUser(id).then(function (result) {
    //        if (result == "success") {
    //            //Success toast here
    //        }
    //        else {
    //            //Error toast here
    //        }
    //    });
    //});
});

//Get user info
var getUserInfo = function (id) {
    return new Promise(function (resolve) {
        $.ajax({
            url: "/User/GetUserByID",
            method: "post",
            data: { id: id },
            success: function (data) {
                resolve(data)
            }
        });
    });
}

////Add user
//var addUser = function (formData) {
//    return new Promise(function (resolve) {
//        $.ajax({
//            url: "/User/Insert",
//            method: "post",
//            data: formData,
//            processData: false,
//            dataType: false,
//            success: function (result) {
//                resolve(result);
//            }
//        })
//    })
//}

////Update user info
//var updateUserInfo = function (formData) {
//    return new Promise(function (resolve) {
//        $.ajax({
//            url: "/User/Update",
//            method: "post",
//            data: formData,
//            processData: false,
//            dataType: false,
//            success: function (result) {
//                resolve(result);
//            }
//        });
//    });
//}

////Delete user
//var deleteUser = function (id) {
//    return new Promise(function (resolve) {
//        $.ajax({
//            url: "/User/Delete",
//            method: "post",
//            data: { id: id },
//            dataType: "json",
//            success: function (result) {
//                resolve(result);
//            }
//        });
//    });
//}

//Reset edit form
//var resetEditForm = function () {
//    $("#user-id").val();
//    $("#user-name").val();
//    $("#student-code").val();
//    $("#full-name").val();
//    $("#password").val();
//    $("#confirm-password").val();
//    $("#birthday").val();
//    $("#email").val();
//    $("#join-date").val();
//    $("#gender").val();
//    $("#status").val();
//}