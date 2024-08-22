let link = document.getElementById("upload-link");
var input = document.getElementById("upload-photo");


link.onclick = async function file() {
    $(input).trigger('click'); 
}

input.onchange = async function s() {
    let formData = new FormData();
    let selectedFile = input.files[0];
    formData.append("file", selectedFile);

    try {
        const res = await fetch("/ChangeBackImage", {
            method: "POST",
            body: formData
        });
        //refresh page so the new background image appears
        location.reload();
        return res;
    }
    catch (ex) {
        console.log(ex)
    }
}
