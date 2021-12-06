$(document).ready(function () {
    //Get user info to edit form
    $(".edit-user-action").on("click", function () {
        var id = $(this).data("id");
        getUserInfo(id).then(function (data) {
            $("#user-id").val(data.userID);
            $("#user-name").val(data.studentCode);
            $("#student-code").val(data.studentCode);
            $("#full-name").val(data.fullname);
            //$("#password").val(data.userID);
            //$("#confirm-password").val(data.userID);
            $("#birthday").val(data.birthDayString);
            $("#email").val(data.email);
            $("#join-date").val(data.joinDate);
            $("#gender").val(data.gender);
            $("#position").val(data.positionID);
            $("#group").val(data.groupID);
            //$("#status").val(data.status);
        });
    });
    //Reset form when exit
    $(".edit-close").on("click", function () {
        document.getElementById("editUser").reset();
    });
    //Send updated info to server to update user info
    $("#editUser").on("submit", function () {
        //var formData = new FormData(this);
        //call update user info here
    });
    //Delete option selected
    $(".delete-user-action").on("click", function () {
        $(".delete-btn").data("id") = $(this).data("id");
    });
    //Delete confirmed
    $(".delete-btn").on("click", function () {
        var id = $(this).data("id");
        deleteUser(id).then(function (result) {
            if (result == "success") {
                //Success toast here
            }
            else {
                //Error toast here
            }
        });
    });
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

//Add user
var addUser = function (formData) {
    return new Promise(function (resolve) {
        $.ajax({
            url: "/User/Insert",
            method: "post",
            data: formData,
            processData: false,
            dataType: false,
            success: function (result) {
                resolve(result);
            }
        })
    })
}

//Update user info
var updateUserInfo = function (formData) {
    return new Promise(function (resolve) {
        $.ajax({
            url: "/User/Update",
            method: "post",
            data: formData,
            processData: false,
            dataType: false,
            success: function (result) {
                resolve(result);
            }
        });
    });
}

//Delete user
var deleteUser = function (id) {
    return new Promise(function (resolve) {
        $.ajax({
            url: "/User/Delete",
            method: "post",
            data: { id: id },
            dataType: "json",
            success: function (result) {
                resolve(result);
            }
        });
    });
}

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