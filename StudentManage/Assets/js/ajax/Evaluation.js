document.addEventListener("DOMContentLoaded", function () {
    const form = document.getElementById("formEvaluation");
    form.onsubmit = function () {
        var formData = new FormData(this);
        $.ajax({
            type: "POST",
            enctype: "multipart/form-data",
            url: "/Evaluation/Detail",
            data: formData,
            processData: false,
            contentType: false,
            cache: false,
            timeout: 600000,
        }).done(data => {
            if (data) {
                MESSAGE.soundSuccess();
                toastr.success(MESSAGE.evaluationSuccess.content, MESSAGE.evaluationSuccess.title);
                window.location = "/Evaluation";
            } else {
                toastr.error(MESSAGE.evaluationError.title, MESSAGE.evaluationError.content);
            }
        });
        
        return false;
    }
    
})