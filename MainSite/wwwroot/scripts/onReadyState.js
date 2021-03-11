//Установка высоты блока , как у родительского
function setHeightChildrenBlock(el) {
    var element = document.getElementById(el);
    var parentElement = element.parentElement.offsetHeight;
    if (parentElement == 0) parentElement = window.innerHeight - 120;

    element.style.minHeight = parentElement + "px";
}

function eventClickMobileIconMenu() {
    //выбираем нужные элементы
    var el = document.getElementById('openMenu');
    el.onclick = function(e) {
        var secondMenuHtml = document.querySelector('.secondMenu');
        var secondMenuBlock = document.querySelector('.secondMenuBlock');
        secondMenuBlock.innerHTML = secondMenuHtml.innerHTML;
        var menuBLock = document.getElementById('menuBlock');
        menuBLock.classList.toggle('active');
     };
}

document.addEventListener('DOMContentLoaded', function() {
    var elems = document.querySelectorAll('.dropdown-trigger');
    var dropdownOptions = {
        inDuration: 500,
        outDuration: 425,
        constrain_width: true,
        coverTrigger: false,// Does not change width of dropdown to that of the activator
       // hover: true, // Activate on hover
        //gutter: (document.getElementsByClassName('dropdown-content')[0].width *3)/2.5 + 5, // Spacing from edge
        belowOrigin: false, // Displays dropdown below the button
        alignment: 'center' // Displays dropdown with edge aligned to the left of button
    };
    var instances = M.Dropdown.init(elems, dropdownOptions);
});

document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('select');
    var instances = M.FormSelect.init(elems);
});

eventClickMobileIconMenu();
setHeightChildrenBlock("mainBlock");