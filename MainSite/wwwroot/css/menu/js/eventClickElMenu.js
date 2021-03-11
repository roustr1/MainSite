
function eventClickElementMenu() {
    //выбираем нужные элементы
    var a = document.querySelectorAll('.menu a');
    
    //перебираем все найденные элементы и вешаем на них события
    [].forEach.call(a, function (el) {
        //вешаем событие
        el.onclick = function(e) {
            var checkFirstNode = el.classList.contains('active');//el.parentElement.parentElement.classList.contains('menu');
            if(!checkFirstNode) {
                var activeElementsFirstChildrensMainNode = el.parentElement.querySelectorAll('*.active');
                if(activeElementsFirstChildrensMainNode.length > 0) {
                    activeElementsFirstChildrensMainNode.forEach(function (element) { element.classList.remove('active'); });
                    return null;
                }
            }
            var items = document.querySelectorAll('.menu ul, .menu a, .menu li');
            for (var itemIndex in items)
                if (typeof items[itemIndex].classList != "undefined") items[itemIndex].classList.remove('active');
            !el.classList.contains('active') ? SearchParent(el, '.menu'):  SearchParent(el, 'li');
        }
    });
}

function SearchParent(node, searchParent) {    
    if(node.parentElement.children.length == 1) node.classList.add('active');
    while (node) {
        if (node.matches(searchParent))
        { 
            return node; 
        }
        else {
            node = node.parentElement;
            for (var i = 0; i < node.children.length; i++) {
                if (node.children[i].nodeName.toLowerCase() == "ul") {
                  node.children[i].classList.add('active'); 
                  break;
                }        
            }
        }
    }
    return null;
}

eventClickElementMenu();