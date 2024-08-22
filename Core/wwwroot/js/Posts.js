
async function Like(id, returnurl) {

    let obj = {
        id: id
    };

    await fetch("/User/Posts/Like", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(obj)
    })
        .catch(err => {

            console.log(err.status);
        });

    location.reload();

}

async function Dislike(id, returnUrl) {

    let obj = {
        id: id
    };

    await fetch("/User/Posts/Dislike", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(obj)
    });

    location.reload();
}

async function AddToBookmarks(id) {

    let obj = {
        id: id
    };

    await fetch("/User/Posts/AddToBookmarks", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(obj)
    });

    location.reload();

    //$.ajax({
    //    url: "/User/Posts/AddToBookmarks",
    //    data: { "id": id, "returnUrl": returnUrl },
    //    success: function (res) { location.href = res.url; },
    //    error: function (err) { alert(err.status) }
    //});
}

async function RemoveFromBookmarks(id) {

    let obj = {
        id: id
    };

    await fetch("/User/Posts/RemoveFromBookmarks", {
        method: 'POST',
        headers: { 'Content-Type': 'application/json'},
        body: JSON.stringify(obj)
    });

    location.reload();
}


async function AddComment(id) {

    let comment = document.getElementById(id).value;
    let object = { postId: id, comment: comment };
    let json = JSON.stringify(object);
    
    await fetch("/User/Posts/AddComment", {
        method:'POST',
        headers: { 'Content-Type': 'application/json' },
        body: json
    });

    location.reload();
}



