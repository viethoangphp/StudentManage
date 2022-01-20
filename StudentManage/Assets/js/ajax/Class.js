$(document).ready(function () {
    $("#form_add").on("submit", function () {
        var formData = new FormData(this)
        addClassAjax(formData).then((data) => {
            if (data == "true") {
                toastr.success("Thêm lớp thành công", "Success");
                sound("/Assets/mp3/smallbox.mp3");
                table.ajax.reload(null, false)
                $("#close-btn").click();
                $("#reset-btn").click();
            } else if(data == "dup") {
                toastr.error("Lớp này đã tồn tại", "Error");
                sound("/Assets/mp3/error.mp3");
            } else {
                toastr.error("Dữ liệu không hợp lệ\nHãy chắc chắn bạn đã điền đầy đủ các trường", "Error");
                sound("/Assets/mp3/error.mp3");
            }
        })
        return false;
    })
    $("#form_edit").on("submit", function () {
        var formData = new FormData(this)
        editClassAjax(formData).then((data) => {
            if (data == "true") {
                toastr.success("Cập nhật khoa thành công", "Success");
                sound("/Assets/mp3/smallbox.mp3");
                table.ajax.reload(null, false)
                $("#close-edit-btn").click();
                $("#reset-edit-btn").click();
            } else if (data == "dup") {
                toastr.error("Lớp này đã tồn tại", "Error");
                sound("/Assets/mp3/error.mp3");
            } else {
                toastr.error("Dữ liệu không hợp lệ\nHãy chắc chắn bạn đã điền đầy đủ các trường", "Error");
                sound("/Assets/mp3/error.mp3");
            }
        })
        return false;
    })
    $(document).on("click", ".editing-btn", function () {
        var id = $(this).data("id");
        getClassAjax(id).then((data) => {
            $("#edit_id").val(data.classID);
            $("#edit_name").val(data.className);
            $("#edit_faculty").val(data.facultyID);
            $("#edit_faculty").trigger('change');
        })
    })
    $(document).on("click", ".delete-dialog", function () {
        var id = $(this).data("id");
        $("#delete-btn").attr("data-id", id);
    })
    $(document).on("click", "#delete-btn", function () {
        var id = $(this).data("id");
        deleteClassAjax(id).then((data) => {
            if (data == "true") {
                toastr.success("Xóa khoa thành công", "Success");
                sound("/Assets/mp3/smallbox.mp3");
                $("#dismiss-delete").click();
                table.ajax.reload(null, false)
            } else {
                toastr.error("Đã có lỗi xảy ra. Vui lòng thử lại", "Error");
                sound("/Assets/mp3/error.mp3");
            }
        })
    })
    var addClassAjax = function (formData) {
        return new Promise((resolve) => {
            $.ajax({
                url: "/Class/Insert",
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
    var editClassAjax = function (formData) {
        return new Promise((resolve) => {
            $.ajax({
                url: "/Class/Update",
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
    var getClassAjax = function (id) {
        return new Promise((resolve) => {
            $.ajax({
                url: "/Class/Get",
                method: "post",
                data: { id: id },
                dataType: "json",
                success: function (data) {
                    resolve(data)
                }
            })
        })
    }
    var deleteClassAjax = function (id) {
        return new Promise((resolve) => {
            $.ajax({
                url: "/Class/Delete",
                method: "post",
                data: { id: id },
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
            { "data": "className", "class": "text-center" },
            { "data": "facultyName", "class": "text-center" },
            { "data": "totalStudent", "class": "text-center" },
            {
                "data": "classID",
                "class": "text-right",
                "render": function (data) {
                    return "<div class='dropdown dropdown-action'> <a href='#' class='action-icon dropdown-toggle' data-toggle='dropdown' aria-expanded='false'><i class='material-icons'>more_vert</i></a><div class='dropdown-menu dropdown-menu-right'> <a class='dropdown-item view-btn' href='/Class/List/" + data + "'><i class='fa fa-eye m-r-5'></i> Xem DSSV</a> <a class='dropdown-item editing-btn' data-id='" + data + "' href='#' data-toggle='modal' data-target='#edit_major'><i class='fa fa-pencil m-r-5'></i> Cập nhật</a> <a class='dropdown-item delete-dialog' data-id='" + data +"' href='#' data-toggle='modal' data-target='#delete_major'><i class='fa fa-trash-o m-r-5'></i> Xóa</a></div></div>";
                }
            }
        ],
        ajax: {
            url: "/Class/GetData",
            method: "POST"
        }
    })
    $("#btn-search").on("click", function () {
        var facultyID = $("#facultyID").val()
        var className = $("#className").val();
        table.ajax.url("/Class/Search?&facultyID=" + facultyID + "&className=" + className).load();
    })

  
})