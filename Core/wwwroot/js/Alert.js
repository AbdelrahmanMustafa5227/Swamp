var friendtobedeleted = null;


$('.btn1').click(function () {
    $('.pop-up').css({
        "opacity": "0", "pointer-events": "none"
    });
    $('.blur').css({
        "filter": "blur(0)", "pointer-events": "auto"
    });

})

$('.btn2').click(function () {
    $('.pop-up').css({
        "opacity": "0", "pointer-events": "none"
    });
    window.location.replace('https://localhost:7084/User/Friends/RemoveFromFriends/' + friendtobedeleted);
})

function check(sender) {

    $('.pop-up').css({
        "opacity": "1", "pointer-events": "auto"
    });
    $('.blur').css({
        "filter": "blur(5px)", "pointer-events": "none"
    });

    friendtobedeleted = $(sender).attr('data-fId');
}

