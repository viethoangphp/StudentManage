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
function ClearData() {
    $("#user_add input").val("")
    $("#user_add select").val("0").change()
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
                "data": "status",
                "class": "text-center",
                "render": function (data, type, row, meta) {
                    if (data == 1) {
                        return "<div class='dropdown action-label'>" +
                            "<a class='btn btn-white btn-sm btn-rounded dropdown-toggle' href='#' " +
                            "data-toggle='dropdown' aria-expanded='false'>" +
                            "<i class='fa fa-dot-circle-o text-success m-r-5'></i>Đã nộp" +
                            "</a>" +
                            "<div class='dropdown-menu dropdown-menu-right'>" +
                            "<a class='dropdown-item selectItem' href='#' data-id=" + row.id + ">" +
                            "<i class='fa fa-dot-circle-o text-danger m-r-5'></i>Đã rút" +
                            "</a>" +
                            "</div>" +
                            "</div>";
                    }
                    return "<div class='dropdown action-label'>" +
                        "<a class='btn btn-white btn-sm btn-rounded dropdown-toggle' href='#' " +
                        "data-toggle='dropdown' aria-expanded='false'>" +
                        "<i class='fa fa-dot-circle-o text-danger m-r-5'></i>Đã rút" +
                        "</a>" +
                        "<div class='dropdown-menu dropdown-menu-right'>" +
                        "<a class='dropdown-item selectItem' href='#' data-id=" + row.id + ">" +
                        "<i class='fa fa-dot-circle-o text-success m-r-5'></i>Đã nộp" +
                        "</a>" +
                        "</div>" +
                        "</div>";
                }
            },
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
    loadIndex();
    $(document).on("click", ".selectItem", function () {
        var id = $(this).data("id")

        var item = this.parentNode.parentNode.querySelector("a").innerHTML;
        console.log(item);
        this.parentNode.parentNode.querySelector("a").innerHTML = this.innerHTML;
        this.innerHTML = item;
        $.ajax({
            url: "/Union/ChangeStatus",
            method: "POST",
            data: { id: id },
            dataType: "json",
            success: function (data) {
                if (data) {
                    sound("/Assets/mp3/smallbox.mp3");
                    toastr.success("Cập Nhật Thành Công", "Thành Công!");
                } else {
                    sound("/Assets/mp3/error.mp3");
                    toastr.error("Lỗi Hệ Thống", "Lỗi!");
                }
            }
        })
    })
    //Notify that feature not available yet
    $(document).on("click", ".editing-btn", function () {
        toastr.error("Tính năng chưa hoàn thiện. Vui lòng thử lại sau", "Lỗi");
    })


  
})