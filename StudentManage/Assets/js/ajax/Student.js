$(document).ready(function () {
    $("#edit_student").on("click", function () {
        updateStudent(formData).then(function (data) {
            
        });
    });
    
})

var updateStudent = function (formData) {
    return new Promise(function(resolve, reject){
        $.ajax({
            url: "/Student/Update",
            method: "post",
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                resolve(data);
            }
        })
    })
}