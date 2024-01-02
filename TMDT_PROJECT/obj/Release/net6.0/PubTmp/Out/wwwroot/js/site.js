

$(document).on("click", ".addProduct", function (e) {
    var value = e.target.getAttribute("data-value");
    
    $.ajax({
        url: "/Cart/AddToCart",
        type: "POST",
        data: { gameid: value },
        success: (response) => {
            alert(response);
        }
    })
});