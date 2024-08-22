let title = document.getElementById("postTitle");
let body = document.getElementById("postBody");
let btn = document.getElementById("createPostBtn");

btn.onclick = async function () {
    let formData = new FormData();
    formData.append("title", title.value);
    formData.append("body", body.value);

    await fetch("/CreatePost", {
        method: "POST",
        body: formData
    });
    location.reload();
}

