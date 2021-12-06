//Export Excel
$(document).ready(function () {
    $(".excel-btn").on("click", function () {
        var id = $(this).data("id");
        location.href = "/Evaluation/ExportExcel/" + id;
    });
});