$("#union_add").on("submit", function () {
    var formData = new FormData(this)
    addUnionAjax(formData).then(function (data) {
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

var addUnionAjax = function (formData) {
    return new Promise(function (resolve) {
        $('#loader').fadeIn('slow');
        $('#loader-wrapper').fadeIn('slow');
        $.ajax({
            url: "/Union/Insert",
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
$("#modal-add-union-close").on("click", function () {
    ClearData();
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
    "columnDefs": [
        { "visible": false, "targets": 0 }
    ],
    columns: [
        { "data": "id" ,"class":"text-center"},
        { "data": "unionID", "class": "text-center" },
        { "data": "fullname" ,"class": "text-center"},
        { "data": "studentCode", "class": "text-center" },
        { "data": "className", "class": "text-center"},
        { "data": "facultyName", "class": "text-center"},
        { "data": "create_at", "class": "text-center"},
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
            "class": "text-right",
            "render": function (data) {
                return "<div class='dropdown dropdown-action'> <a href='#' class='action-icon dropdown-toggle' data-toggle='dropdown' aria-expanded='false'><i class='material-icons'>more_vert</i></a> <div class='dropdown-menu dropdown-menu-right'> <a class='dropdown-item btn-update' href='/Union/Print/" + data + "'><i class='fa fa-print m-r-5'></i> In Biểu Mẫu </a><a class='dropdown-item sendReturnEmail' href='#'  data-id='" + data +"'><i class='fa fa-envelope m-r-5'></i> Gửi Lại Email Nộp Sổ Đoàn</a></div></div>";
            }
        }
    ],

    ajax: {
        url: "/Union/GetData",
        method: "POST"
    }
})
loadIndex();
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
                $("#myTable").DataTable().ajax.reload();
            } else {
                sound("/Assets/mp3/error.mp3");
                toastr.error("Lỗi Hệ Thống", "Error!");
            }
        }
    })
})
$(document).ready(function () {
    var conuuu = 0;
    // Insert 
    $("#ImportExcel").on("click", function () {
        const fileName = document.getElementById("fileName");
        fileName.click();
    })
    fileName.addEventListener("change", function () {
        console.log("Change" + conuuu);
        conuuu++;
        var file = fileName.value;
        var typeFile = file.split(".")[file.split(".").length - 1];
        if (typeFile != "xlsx") {
            alert("File Không Đúng Định Dạng");
            fileName.value = "";
        } else {
            readXlsxFile(fileName.files[0]).then(function (rows) {
                var listObj = [];
                for (let i = 1; i < rows.length; i++) {
                    var obj = new Object();
                    obj.unionID = "" + rows[i][0] + "";
                    obj.fullname = ""+rows[i][1]+"";
                    obj.studentCode = ""+rows[i][2]+"";
                    obj.phone = ""+rows[i][3]+"";
                    obj.email = rows[i][4];
                    obj.className = rows[i][5];
                    obj.facultyName = rows[i][6];
                    listObj.push(obj);
                }
                console.log(listObj);
                listObj = JSON.stringify({ 'list': listObj });
                InsertRows(listObj)
                    .then(function (data) {
                        loaderFade();
                        if (data.error > 0) {
                            toastr.error("Import Error " + data.error + " rows !", "Error!");
                            window.location = "/Union/DownloadErrorHighlight"
                        }
                        if (data.success > 0) {
                            toastr.success("Import " + data.success + " rows !", "Success!");
                        }    
                        $("#myTable").DataTable().ajax.reload();
                    })
            })
        }
    });
    const InsertRows = function (listObj) {
        return new Promise(function (resolve) {
            $('#loader').fadeIn('slow');
            $('#loader-wrapper').fadeIn('slow');
            $.ajax({
                url: "/Union/InsertExcel",
                method: "POST",
                data: listObj,
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    resolve(data);
                }
            })
        });
    }
    $("#btn-search").on("click", function () {
        var classid = $("#class").val();
        var status = $("#status").val();
        var unionid = $("#unionid").val();
        var facultyId = $("#faculty").val()
        var semester = $("#semester").val();
        table.ajax.url("/Union/Search?classId=" + classid + "&status=" + status + "&unionId=" + unionid + "&facultyId=" + facultyId +"&semester="+semester).load();
       

    })
    $("#ExportExcel").on("click", function () {
        var classid = $("#class").val();
        var status = $("#status").val();
        var unionid = $("#unionid").val();
        var facultyId = $("#faculty").val()
        var semester = $("#semester").val();
        toastr.info("File Đang Được Tải Xuống Vui Lòng Chờ Trong Giây Lát !", "Đợi Xíu Nha Thím");
        window.location = "/Union/ExportExcel?classId=" + classid + "&status=" + status + "&unionId=" + unionid + "&facultyId=" + facultyId + "&semester=" + semester;


    })
    const listClass = document.getElementById("class");
    $("#faculty").on("change", function () {
        console.log("Change");
        $("#unionid").val("");
        listClass.length = 1;
        var facultyId = $(this).val();
        var year = $("#semester").val();
        $.ajax({
            url: "/Union/GetClassData",
            method: "POST",
            data: { facultyId: facultyId, year: year },
            dataType: "json",
            success: function (data) {
                console.log(listClass);
                data.forEach(item => {
                    listClass.options[listClass.options.length] = new Option(item.className, item.classID);
                })
                console.log(data);
            }
        })
    })
    $("#semester").on("change", function () {
        $('#faculty').val('0'); // Select the option with a value of '1'
        $('#faculty').trigger('change');
        listClass.length = 1;
        $("#unionid").val("");

    })
    $(document).on("click",".sendReturnEmail", function () {
        console.log("click");

        sendReturnEmail($(this).data("id")).then(function (data) {
            loaderFade();
            toastr.success("Gửi Email Thành Công", "Success!");
        })
    })
    const sendReturnEmail = function (id) {
        return new Promise(function (resolve) {
            $('#loader').fadeIn('slow');
            $('#loader-wrapper').fadeIn('slow');
            $.ajax({
                url: "/Union/SendEmail",
                method: "POST",
                data: { id: id },
                dataType: "json",
                success: function (data) {
                    resolve(data)
                }
            })
        })
    }
})
function loadIndex() {
    $("#myTable_filter").css("display", "none");
    table.on('order.dt search.dt', function () {
        table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();
}

