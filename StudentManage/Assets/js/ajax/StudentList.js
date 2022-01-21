//const { success } = require("toastr");

$(document).ready(function () {
    //Update student 
    $("#student_update").on("submit", function () {
        var formData = new FormData(this);
        UpdateStudent(formData).then(function (data) {
            loaderFade();
            setTimeout(function () {
                if (data > 0) {
                    sound("/Assets/mp3/smallbox.mp3");
                    toastr.success("Cập Nhật Thành Công !", "Thành Công!");
                    $("#edit_student").delay(500).modal("hide");
                    $("#myTable").DataTable().ajax.reload();
                } else if (data == "dup") {
                    sound("/Assets/mp3/error.mp3");
                    toastr.error("Cập nhật không thành công", "Lỗi!");
                    document.getElementsByName("studentCode")[0].value = "";
                } else if (data == "false") {
                    sound("/Assets/mp3/error.mp3");
                    toastr.error("Bạn Không Được Để Trống Những Trường Dữ Liệu Có Dấu (*) . Vui lòng kiểm tra và thử lại .Thanks", "Lỗi!");
                } else if (data == "errorEmail") {
                    sound("/Assets/mp3/error.mp3");
                    toastr.error("Email Không Hợp Lệ . Vui lòng kiểm tra và thử lại .Thanks", "Lỗi!");
                } else {
                    sound("/Assets/mp3/error.mp3");
                    toastr.error("Mã Số Sinh Viên Không Hợp Lệ ", "Lỗi!");
                }
            }, 1500)
        });
        return false;
    });

    var UpdateStudent = function (formData) {
        return new Promise(function (resolve) {
            $('#loader').fadeIn('slow');
            $('#loader-wrapper').fadeIn('slow');
            $.ajax({
                url: "/Student/Update",
                type: "POST",
                data: formData,
                processData: false,
                contentType: false,
                cache: false,
                timeout: 600000,
                success: function (data) {
                    resolve(data);
                }
            })
        })
    }


    //
    var arr = (window.location.pathname).split("/");
    var val = (arr[arr.length - 1]);

    var table = $("#myTable").DataTable({
        searching: false,
        ordering: false,
        responsive: true,
        processing: true,
        serverSide: true,
        //language: {
        //    "lengthMenu": "Display _MENU_ records per page",
        //    "zeroRecords": "Không Có Dữ Liệu Nào Thím Ạ :(((",
        //    "info": "Đang ở trang _PAGE_/_PAGES_ thím ạ ahihi",
        //    "infoEmpty": "Không Tìm Thấy",
        //    "infoFiltered": "(filtered from _MAX_ total records)",
        //    "loadingRecords": "Đang Tải Dữ Liệu Vui Lòng Chờ Trong Giây Lát ahihi...",
        //    "processing": "Đợi Chút Xíu Có Liền Ngay Cho Thím ahiii...",
        //},
        columns: [
            { "defaultContent": "-", "class": "text-center" },
            { "data": "studentCode", "class": "text-center" },
            { "data": "fullname", "class": "text-center" },
            { "data": "email", "class": "text-center" },
            { "data": "phone", "class": "text-center" },
            {
                "data": "userID",
                "class": "text-right",
                "render": function (data) {
                    return "<div class='dropdown dropdown-action'> <a href='#' class='action-icon dropdown-toggle' data-toggle='dropdown' aria-expanded='false'><i class='material-icons'>more_vert</i></a><div class='dropdown-menu dropdown-menu-right'> <a class='dropdown-item editing-btn' data-id='" + data + "' href='#' data-toggle='modal' data-target='#edit_student'><i class='fa fa-eye m-r-5'></i> Xem/Cập nhật</a></div></div>";
                }
            }
        ],
        ajax: {
            url: "/Class/List/" + val,
            method: "POST"
        }
    });

    //Show 
    $(document).on("click", ".editing-btn", function (e) {
        let id = $(this).data('id');
        update_user(id).then(function (data) {
            console.log(data);
            rederData(data).then(() => {
                $('#edit_student select').trigger("change");
            });

        })
    })

    const update_user = function (id) {
        return new Promise(function (resolve) {
            $.ajax({
                url: "/User/GetUserByID/",
                method: "POST",
                data: { id: id },
                dataType: "json",
                success: function (data) {
                    resolve(data);
                }
            })
        })
    }

    function rederData(responsive) {
        return new Promise((resolve) => {
            //$('#edit_student select[name="cityID"]').val(0);
            const form = document.getElementById("edit_student");
            form.getElementsByTagName("input").fullname.value = responsive.fullname;
            form.getElementsByTagName("input").userID.value = responsive.userID;
            form.getElementsByTagName("input").joinDate.value = responsive.joinDate;
            form.getElementsByTagName("input").studentCode.value = responsive.studentCode;
            form.getElementsByTagName("input").phone.value = responsive.phone;
            form.getElementsByTagName("input").email.value = responsive.email;

            //var strDate = responsive.birthDay; // /Date(974480400000)/
            //var num = parseInt(strDate.replace(/[^0-9]/g, ""));
            //var date = new Date(num);
            //var dmy = ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" + date.getFullYear();
            var dmy = convertTimeStampToD(responsive.birthDay);
            form.getElementsByTagName("input").birthDayString.value = dmy;

            form.getElementsByTagName("input").className.value = responsive.className;
            form.getElementsByTagName("input").address.value = responsive.address;

            $('#edit_student select[name="gender"]').val(responsive.gender);
            $('#edit_student select[name="facultyID"]').val(responsive.facultyID);
            $('#edit_student select').trigger("change");
        })
    }

    function convertTimeStampToD(strDate) { ///Date(974480400000)/
        var num = parseInt(strDate.replace(/[^0-9]/g, ""));
        var date = new Date(num);
        return dmy = ("0" + date.getDate()).slice(-2) + "/" + ("0" + (date.getMonth() + 1)).slice(-2) + "/" + date.getFullYear();
    }

    $("#close-btn").on("click", function () {
        ClearData();
    })

    function ClearData() {
        $("#student_update input").val("")
        $("#student_update select").val("0").change()
    }
})

