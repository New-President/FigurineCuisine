// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
$(document).ready(function () {
    $('.minus').click(function () {
        var $input = $(this).parent().find('.quantity');
        var count = parseInt($input.val()) - 1;
        count = count < 1 ? 1 : count;
        $input.val(count);
        $input.change();
        return false;
    });
    $('.plus').click(function () {
        var $input = $(this).parent().find('.quantity');
        $input.val(parseInt($input.val()) + 1);
        $input.change();
        return false;
    });
});

function dateFunction() {
    var today = new Date();
    var mm = today.getMonth() + 1;
    var yyyy = today.getFullYear();
    if (mm < 10) {
        mm = "0" + mm;
    }
    var sToday = yyyy + "-" + mm
    document.getElementById("myDate").setAttribute("min", sToday);

}
