
var cover = document.querySelector('.thecover');
  /*  infopage = document.querySelector('.infopage');*/

addSwipeEvent(cover, "swipeUp", function () {
    cover.classList.remove("active");
   /* infopage.classList.add("active");*/
    //window.open("http://192.168.1.12:8082", "_self");
    window.parent.postMessage('swipeRight', 'http://localhost:4200');
});


