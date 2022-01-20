$(document).on("submit", "#user_add", function () {
   
    var formData = new FormData(this)
    addUserAjax(formData).then(function (data) {
        loaderFade();
        setTimeout(function () {
            if (data > 0) {
                sound("/Assets/mp3/smallbox.mp3");
                toastr.success("Thêm Thành Công !", "Success!");
                $("#myTable").DataTable().ajax.reload();
                ClearData();
                $("#modal-add-union-close").click();
                setTimeout(function () {
                    $("#btn-show-dialog").click();
                    $("#print__").attr("href", "/Union/Print/" + data);
                }, 1000)
            } else if (data == "dup") {
                sound("/Assets/mp3/error.mp3");
                toastr.error("Mã Số Sinh Viên Đã Tồn Tại Trong Hệ Thống", "Error!");
                document.getElementsByName("studentCode")[0].value = "";
            } else if (data == "false") {
                sound("/Assets/mp3/error.mp3");
                toastr.error("Bạn Không Được Để Trống Những Trường Dữ Liệu Có Dấu (*) . Vui lòng kiểm tra và thử lại .Thanks", "Error!");
            } else if (data == "errorEmail") {
                sound("/Assets/mp3/error.mp3");
                toastr.error("Email Không Hợp Lệ . Vui lòng kiểm tra và thử lại .Thanks", "Error!");
            } else {
                sound("/Assets/mp3/error.mp3");
                toastr.error("Mã Số Sinh Viên Không Hợp Lệ ", "Error!");
            }
        }, 1500)
    })
    return false;
})
var addUserAjax = function (formData) {
    return new Promise(function (resolve) {
        $('#loader').fadeIn('slow');
        $('#loader-wrapper').fadeIn('slow');
        $.ajax({
            url: "/Class/InsertUserByClass",
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
$(document).ready(function () {
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
    })
    //Notify that feature not available yet
    $(document).on("click", ".editing-btn", function () {
        toastr.error("Tính năng chưa hoàn thiện. Vui lòng thử lại sau", "Lỗi");
    })


  
})