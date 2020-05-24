function getCookie(name) {
    let matches = document.cookie.match(new RegExp(
        "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
    ));
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

window.addEventListener("load", function (event) {

    let Rights = getCookie("Rights");

    if (Rights == "No") {
        $(".User").addClass("disabled");
        $(".Admin").addClass("disabled");
    }

    if (Rights == "User") {
        $(".Admin").addClass("disabled");
    }
});
