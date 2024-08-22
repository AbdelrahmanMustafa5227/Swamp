let btn = document.getElementById("upload-propic-btn");
let input = document.getElementById("upload-propic");

btn.onclick = function () {
    $(input).trigger('click');
}

input.onchange = async function () {
    let formData = new FormData();
    let value = input.files[0];
    formData.append('file', value);

    await $.ajax({
        url: '/changeProfilePicture',
        method: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (res) {
            location.href = res.redirectToUrl;
        },
        error: function (err) { alert(err); }
    });
}


function AssignDefaultPic(x) {
    $.ajax({
        url: "/User/Profile/UseDefaultImage",
        data: { num: x },
        success: function (res) { location.href = res.redirectToUrl; }
    })
}
