var slider = document.getElementById("myRange");
var element = document.querySelector('.image-cropped');


// Update the current slider value (each time you drag the slider handle)
slider.oninput = function asd() {

    var isLandscape = $(slider).attr('data-IsWidthBigger');

    if (isLandscape === "True") {
        
        var width = $(element).attr('data-widthDifference');
        var ConvertPercentToPixels = slider.value * width / 100;
        var margin = (-ConvertPercentToPixels) + "px";
        var translate = "translate(" + ConvertPercentToPixels + "px , -300px)";

        $('.image-cropped').css({
            "position": "relative",
            "margin-left": margin
        });

        $('.image-overflow').css({
            "transform": translate
        });

    }
    else {
        
        var heigth = $(element).attr('data-ImgHeigth');
        var heigthDifference = $(element).attr('data-HeigthDiff');
        var ConvertPercentToPixels = slider.value * heigthDifference / 100;
        var margin = (-ConvertPercentToPixels) + "px" + " auto";
        var translate = "translate(0px ," + (-heigth + ConvertPercentToPixels) + "px)";

        $('.image-cropped').css({
            "position": "relative",
            "margin": margin,
        });

        $('.image-overflow').css({
            "transform": translate
        });

    }
    
}