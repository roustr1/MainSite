
function getAllCheckBox() {
    var elements = document.querySelectorAll('.selectAllCheckbox');

    for (var i = 0; i < elements.length; i++) {
        setAction(i);
    }

    function setAction(index) {
        elements[index].parentElement.addEventListener('click', function (e) {
            var currentMainInput = elements[index];
            currentMainInput.checked = !currentMainInput.checked; 

            var trList = document.querySelectorAll('table tr');
            for (var j = 1; j < trList.length; j++) {
                var currentTd = trList[j].children[index + 1];
                var inputCurrentTd = searchInput(currentTd);

                if (inputCurrentTd != null) {
                    if (!currentMainInput.checked) {
                        inputCurrentTd.removeAttribute("checked");
                    }
                    else {
                        inputCurrentTd.setAttribute("checked", "checked");
                    }
                }
            }
        })
    }

    function searchInput(node) {
        var result;
        for (var k = 0; k < node.children.length; k++) {
            if (node.children[k].getAttribute('type') == "checkbox") {
                return node.children[k];
            }

            result = searchInput(node.children[k]);

            if (result != null) {
                return result;
            }
        }

        return null;
    }
}

getAllCheckBox();