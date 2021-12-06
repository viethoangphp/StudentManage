const wrapper = document.querySelector(".upload-pic_wrapper");
         const fileName = document.querySelector(".upload-pic_file-name");
         const defaultBtn = document.querySelector("#upload-pic_default-btn");
         const customBtn = document.querySelector("#upload-pic_custom-btn");
         const cancelBtn = document.querySelector("#upload-pic_cancel-btn i");
         const img = document.querySelector(".upload-pic_image img");
         let regExp = /[0-9a-zA-Z\^\&\'\@\{\}\[\]\,\$\=\!\-\#\(\)\.\%\+\~\_ ]+$/;
         function defaultBtnActive(){
           defaultBtn.click();
         }         
         defaultBtn.addEventListener("change", function(){
           const file = this.files[0];
           let fileType = file.type;
           let validExtensions = ["image/jpeg", "image/jpg", "image/png"];
           if(validExtensions.includes(fileType)){
             const reader = new FileReader();
             reader.onload = function(){
               const result = reader.result;
               img.src = result;
               wrapper.classList.add("active");
             }
             cancelBtn.addEventListener("click", function(){
                if(img.classList.contains('ava-img')){
                    img.src = "assets/img/user.jpg";
                    wrapper.classList.remove("active");
                }else{
                    img.src = "";
                    wrapper.classList.remove("active");
                }               
             })
             reader.readAsDataURL(file);
           }
           else{
            alert("Đây không phải là file ảnh!");
           }
           if(this.value){
             let valueStore = this.value.match(regExp);
             fileName.textContent = valueStore;
           }
           
         });