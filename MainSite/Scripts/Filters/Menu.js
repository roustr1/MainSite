export const ItemMenuActive = {
    eventClickElementMenu: function (el) {
        el = el.target.tagName !== 'A' ? el.target.parentNode: el.target;
        /*if(!checkFirstNode) {
            var activeElementsFirstChildrensMainNode = el.parentElement.querySelectorAll('*.active');
            if (activeElementsFirstChildrensMainNode.length > 0) {
                for (var elementIndex in activeElementsFirstChildrensMainNode) {
                    if (typeof activeElementsFirstChildrensMainNode[elementIndex].classList != "undefined")
                        activeElementsFirstChildrensMainNode[elementIndex].classList.remove('active');
                }
                return null;
            }
        }*/
        var items = document.querySelectorAll('.menu ul, .menu a, .menu li');
        for (var itemIndex in items)
            if (typeof items[itemIndex].classList != "undefined") items[itemIndex].classList.remove('active');
        !el.classList.contains('active') ? SearchParent(el, '.menu') : SearchParent(el, 'li');
    }
}

var SearchParent = (node, searchParent) => {
    node.classList.add('active');
    //if(node.parentElement.children.length == 1) node.classList.add('active');
    while (node) {
        if (node.matches(searchParent)) {
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