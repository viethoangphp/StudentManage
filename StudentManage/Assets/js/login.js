// login event

const sign_in_btn = document.querySelector("#sign-in-btn");
const sign_up_btn = document.querySelector("#sign-up-btn");
const containerLoginPage = document.querySelector(".container-loginpage");

sign_up_btn.addEventListener("click", () => {
  containerLoginPage.classList.add("sign-up-mode");
});

sign_in_btn.addEventListener("click", () => {
  containerLoginPage.classList.remove("sign-up-mode");
});
document.addEventListener("DOMContentLoaded", function () {
    $("#form-login").on("submit", function () {
        var username = $("#username").val();
        var password = $("#password").val();
        login(username, password).then((data) => {
            loaderFade();
                if (data == "true") {
                    window.location = "/";
                } else {
                    setTimeout(() => {
                        toastr.error("Tên Đăng Nhập Hoặc Mật Khẩu Không Chính Xác !", "Error");
                    }, 1000)
                }
            
        })
        return false;
    })
})
const login = function (username, password) {
    $('#loader').fadeIn('slow');
    $('#loader-wrapper').fadeIn('slow');
    return new Promise(resolve => {
        $.ajax({
            url: "/Login",
            method: "POST",
            data: { username: username, password: password },
            dataType: "json",
            success: function (data) {
                resolve(data);
            }
        })
    })
}
