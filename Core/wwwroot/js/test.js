//create connection
let con = new signalR.HubConnectionBuilder().withUrl('/chat').build();

//recieve notification from hub
con.on("updateViews", (value) => {
    var z = document.getElementById('z');
    z.innerText = value.toString();
});

con.on("updateUsers", (value) => {
    var z = document.getElementById('y');
    z.innerText = value.toString();
});
//send notification to hub
function a() {
    con.send("newWindowLoaded");
}

//start connection
con.start().then(success, rejected);
function success() { console.log("connection is successful"); a(); }
function rejected() { console.log("connection rejected"); }