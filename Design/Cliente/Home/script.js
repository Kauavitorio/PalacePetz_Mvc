//Slideshow com texto e imagem
let time = 5000,
    currentImageWeb = 0,
    currentTexttitWeb = 0,
    currentTextsubWeb = 0,
    text_tit = document.querySelectorAll(".texts h1"),
    text_sub = document.querySelectorAll(".texts h2"),
    images = document.querySelectorAll(".sliders img")
    maximg = images.length;
    maxtexttit = text_tit.length;
    maxtextsub = text_sub.length;

    function nextImage(){
        images[currentImageWeb].classList.remove("selected")

        currentImageWeb++

        if(currentImageWeb >= maximg)
            currentImageWeb = 0

        images[currentImageWeb].classList.add("selected")
    }
    function nextText1(){
        text_tit[currentTexttitWeb].classList.remove("first1")

        currentTexttitWeb++
        
        if(currentTexttitWeb >= maxtexttit){
            currentTexttitWeb = 0
        }  

        text_tit[currentTexttitWeb].classList.add("first1")  
    }
    function nextText2(){
        text_sub[currentTextsubWeb].classList.remove("first2")

        currentTextsubWeb++

        if(currentTextsubWeb >= maxtextsub){
            currentTextsubWeb = 0
        }  

        text_sub[currentTextsubWeb].classList.add("first2")
    }

    function startImage(){
        setInterval(() => {
            //Troca Imagem
            nextImage();
        }, time)
    }

    function startText1(){
        setInterval(() => {
            //Troca text
            nextText1();
        }, time)
    }

    function startText2(){
        setInterval(() => {
            //Troca text
            nextText2();
        }, time)
    }
window.addEventListener("load", startText1)
window.addEventListener("load", startText2)
window.addEventListener("load", startImage)