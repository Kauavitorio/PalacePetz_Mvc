window.onload = function(){
    // All code comes here 
    var spanCountCart = document.getElementById('lblCartCount').innerHTML
    if(spanCountCart == "0" || spanCountCart == "@ViewBag.cartsize" || spanCountCart == "" || spanCountCart == " ")
        document.getElementById('lblCartCount').style.display = "none"
}