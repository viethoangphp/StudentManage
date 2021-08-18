$(document).ready(function () {
    $("#form_add").on("submit", function () {
        var formData = new FormData(this)
        addFacultyAjax(formData).then((data) => {
            if (data != 0) {
                toastr.success("Thêm khoa thành công", "Success");
                $("#close-btn").click();
            } else {
                toastr.error("Đã có lỗi xảy ra. Vui lòng thử lại", "Error");
            }
        })
    })
    var addFacultyAjax = function (formData) {
        return new Promise((resolve) => {
            $.ajax({
                url: "/Faculty/Insert",
                method: "post",
                data: formData,
                contentType: false,
                processData: false,
                dataType: "json",
                success: function (data) {
                    resolve(data)
                }
            })
        })
    }
    var table = $("#myTable").DataTable({
        searching: false,
        ordering: false,
        processing: true,
        serverSide: true,
        language: {
            "lengthMenu": "Display _MENU_ records per page",
            "zeroRecords": "Không Có Dữ Liệu Nào Thím Ạ :(((",
            "info": "Đang ở trang _PAGE_/_PAGES_ thím ạ ahihi",
            "infoEmpty": "Không Tìm Thấy",
            "infoFiltered": "(filtered from _MAX_ total records)",
            "loadingRecords": "Đang Tải Dữ Liệu Vui Lòng Chờ Trong Giây Lát ahihi...",
            "processing": "Đợi Chút Xíu Có Liền Ngay Cho Thím ahiii...",
        },
        columns: [
            null,
            { "data": "facultyName" },
            { "data": "phone" },
        ],
        ajax: {
            url: "/Faculty/GetData",
            method: "POST"
        }
    })
})
