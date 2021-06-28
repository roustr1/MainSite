
function inputFilter() {
    // Declare variables
    function search(name, filter) {
        return name.toUpperCase().trim().startsWith(filter);
    }

    var input, filter, tr, i;
    input = document.getElementById('search');
    filter = input.value.toUpperCase();

    var tbody = document.getElementById("record_list");
    tr = tbody.getElementsByTagName("tr");

    // Loop through all list items, and hide those who don't match the search query
    for (i = 0; i < tr.length; i++) {
        var nameTd = tr[i].getElementsByTagName("td");
     
        if (search(nameTd[0].innerText, filter) || search(nameTd[1].innerText,filter)) {
            tr[i].style.display = "";
        } else {
            tr[i].style.display = "none";
        }
    }
}


 
