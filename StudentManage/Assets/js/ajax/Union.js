$("#union_add").on("submit", function () {
    var formData = new FormData(this)
    $.ajax({
        url: "/Union/Insert",
        type: "POST",
        data: formData,
        processData: false,
        contentType: false,
        cache: false,
        timeout: 600000,
        success: function (data) {
            if (data != "false") {
                sound("/Assets/mp3/smallbox.mp3");
                toastr.success("Thêm Thành Công !", "Success!");
                $("#myTable").DataTable().ajax.reload();
                ClearData();
            } else {
                sound("/Assets/mp3/error.mp3");
                toastr.error("Bạn Không Được Để Trống Những Trường Dữ Liệu Có Dấu (*) . Vui lòng kiểm tra và thử lại .Thanks", "Error!");
            }
        }
    })
    return false;
})
$(".btn-update").on("click", function () {
    var id = $(this).data("id");
    $.ajax({
        url: "/Union/View/",
        method: "POST",
        data: { id: id },
        dataType: "json",
        success: function (data) {
            console.log(data);
        }
    })

})
function ClearData() {
    $("#union_add input").val("")
    $("#union_add select").val("0").change()
}
var table = $("#myTable").DataTable({
    ordering: false,
    columns: [
        { "data": "id" },
        { "data": "unionID" },
        { "data": "fullname" },
        { "data": "studentCode" },
        { "data": "className" },
        { "data": "facultyName" },
        { "data": "create_at" },
        {
            "data": "status",
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
                    "<i class='fa fa-dot-circle-o text-success m-r-5'></i>Đã rút" +
                    "</a>" +
                    "<div class='dropdown-menu dropdown-menu-right'>" +
                    "<a class='dropdown-item selectItem' href='#' data-id=" + row.id + ">" +
                    "<i class='fa fa-dot-circle-o text-danger m-r-5'></i>Đã nộp" +
                    "</a>" +
                    "</div>" +
                    "</div>";
            }
        },
        {
            "data": "id",
            "render": function (data) {
                return "<div class='dropdown dropdown-action'> <a href='#' class='action-icon dropdown-toggle' data-toggle='dropdown' aria-expanded='false'><i class='material-icons'>more_vert</i></a> <div class='dropdown-menu dropdown-menu-right'> <a class='dropdown-item btn-update ' href='#' data-toggle='modal' data-id='@item.id' data-target='#edit_union-notebook'> <i class='fa fa-pencil m-r-5'></i> Cập nhật </a> <a class='dropdown-item' href='#' data-toggle='modal' data-target='#delete_union-notebook'> <i class='fa fa-trash-o m-r-5'></i> Xóa </a> </div></div>";
            }
        }
    ],
    ajax: {
        url: "/Union/GetData",
        method: "POST",
        dataSrc: ""
    }
})
table.on('order.dt search.dt', function () {
    table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
        cell.innerHTML = i + 1;
    });
}).draw();
$(document).on("click", ".selectItem", function () {
    var id = $(this).data("id")
    console.log($(this).data("id"));
    var item = this.parentNode.parentNode.querySelector("a").innerHTML
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
                toastr.success("Cập Nhật Thành Công", "Success!");
            } else {
                sound("/Assets/mp3/error.mp3");
                toastr.error("Lỗi Hệ Thống", "Error!");
            }
        }
    })
})
$(document).ready(function () {
    $("#myTable tbody tr td").addClass("text-center")
    $("#myTable tbody tr>td:last-child").removeClass("text-center").addClass("text-right")
    // Insert 
    $("#ImportExcel").on("click", function () {
        const fileName = document.getElementById("fileName");
        fileName.click();
        fileName.addEventListener("change", function () {
            var file = fileName.value;
            var typeFile = file.split(".")[file.split(".").length - 1];
            if (typeFile != "xlsx") {
                alert("File Không Đúng Định Dạng");
                fileName.value = "";
            } else {
                readXlsxFile(fileName.files[0]).then(function (rows) {
                    InsertRows(rows);
                })

            }
        })
        const ajax = function (row) {
            return new Promise(function (resolve,reject) {
                var obj = {
                    "fullname": row[0],
                    "studentCode": row[1],
                    "phone": row[2],
                    "email": row[3],
                    "className":row[4]
                }
                $.ajax({
                    url: "/Union/InsertExcel",
                    method: "POST",
                    data: obj,
                    dataType: "json",
                    success: function(data){
                        if (data) {
                            resolve();
                        } else {
                            reject(row);
                        }
                    }

                })
            })
        }
        const InsertRow = function (row) {
            return ajax(row).then(function () {
                console.log("Insert A Row Done");
            })
            .catch(function (data) {
                 console.log("Lỗi Dữ Liệu Hàng" + data);
             })
        }
        const InsertRows = async function (rows) {
            console.log('Start')
            for (var i = 1; i < rows.length; i++) {
                var row = await InsertRow(rows[i]);
            }
            sound("/Assets/mp3/smallbox.mp3");
            toastr.success("Thêm Thành Công !", "Success!");
            $("#myTable").DataTable().ajax.reload();
            fileName.value = "";
            console.log("Done");
        }

    })
})