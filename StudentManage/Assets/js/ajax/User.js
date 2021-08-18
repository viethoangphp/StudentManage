$("#ChangePassword").on("submit", function () {
    var passwordOld = $("#passwordOld").val();
    var passwordNew = $("#passwordNew").val();
    var confirmPassword = $("#confirmPassword").val();
    var obj = {
        passwordOld: passwordOld,
        passwordNew: passwordNew,
        confirmPassword: confirmPassword
    }
    $.ajax({
        url: "/Profile/ChangePassword",
        method: "POST",
        data: obj,
        dataType: "json",
        success: function (data) {
            if (data == "true") {
                toastr.success("Đổi Mật Khẩu Thành Công", "Succses!");
                $("#passwordOld").val("");
                $("#passwordNew").val("");
                $("#confirmPassword").val("");
                sound("/Assets/mp3/smallbox.mp3");
                $("#ChangePasswordClose").click();
            } else if (data == "confirmError") {
                toastr.error("Mật Khẩu Mới Không Trùng Khớp", "Error!");
                $("#passwordOld").val("");
                $("#passwordNew").val("");
                $("#confirmPassword").val("");
                sound("/Assets/mp3/error.mp3");
            } else if (data == "passwordError") {
                toastr.error("Mật Khẩu Cũ Không Đúng", "Error!");
                $("#passwordOld").val("");
                $("#passwordNew").val("");
                $("#confirmPassword").val("");
                sound("/Assets/mp3/error.mp3");
            } else {
                toastr.error("Bạn Không Được Để Trống Các Trường Có Dấu (*)", "Error!");
                $("#passwordOld").val("");
                $("#passwordNew").val("");
                $("#confirmPassword").val("");
                sound("/Assets/mp3/error.mp3");
            }
        }

    })
    return false;
})
$("#UpdateProfile").on("submit", function () {
    var formData = new FormData(this);
    $.ajax({
        url: "/Profile/UpdateProfile",
        method: "POST",
        data: formData,
        processData: false,
        contentType: false,
        cache: false,
        timeout: 600000,
        success: function (data) {
            if (data == "true") {
                sound("/Assets/mp3/smallbox.mp3");
                toastr.success("Cập Nhật Dữ Liệu Thành Công", "Success !");
                setTimeout(function () {
                    window.location = "/Profile";
                }, 2000)
            } else if (data == "false") {
                sound("/Assets/mp3/error.mp3");
                toastr.warning("Bạn Không Được Để Trống Các Trường Có Dấu (*)", "Cảnh Báo !");
            } else {
                sound("/Assets/mp3/error.mp3");
                toastr.warning("Email Không Đúng Định Dạng", "Cảnh Báo !");
            }
        }

    })
    return false;
})

